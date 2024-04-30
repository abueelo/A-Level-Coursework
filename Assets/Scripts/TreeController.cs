using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeController : MonoBehaviour
{
    public Dictionary<Vector2, IEnumerator> coroutines = new Dictionary<Vector2, IEnumerator>();
    public Tile treeTile;
    public Tile decayTile;
    private int placedTrees = 0;
    private int maxTrees = 5;
    private Tilemap treeTilemap;

    void Start()
    {
        treeTilemap = GameObject.Find("Obstacles").GetComponent<Tilemap>();
    }

    IEnumerator DecayTree(Vector2 position)
    {
        yield return new WaitForSeconds(1f);

        // changes the tile to the trees decay tile
        treeTilemap.SetTile(treeTilemap.WorldToCell(position), decayTile);
        yield return new WaitForSeconds(0.1f); // waits 0.1 seconds
        treeTilemap.SetTile(treeTilemap.WorldToCell(position), treeTile); // changes it back

        yield return new WaitForSeconds(1f); // waits 1 second

        //repeats the process
        treeTilemap.SetTile(treeTilemap.WorldToCell(position), decayTile);
        yield return new WaitForSeconds(0.1f);
        treeTilemap.SetTile(treeTilemap.WorldToCell(position), treeTile);

        yield return new WaitForSeconds(1f);

        treeTilemap.SetTile(treeTilemap.WorldToCell(position), decayTile);
        yield return new WaitForSeconds(0.1f);
        treeTilemap.SetTile(treeTilemap.WorldToCell(position), treeTile);
        
        yield return new WaitForSeconds(1f);

        treeTilemap.SetTile(treeTilemap.WorldToCell(position), decayTile);
        yield return new WaitForSeconds(0.1f);
        treeTilemap.SetTile(treeTilemap.WorldToCell(position), treeTile);

        treeTilemap.SetTile(treeTilemap.WorldToCell(position), null);
        placedTrees--; // decrements the number of placed trees
        coroutines.Remove(position);
    }

    public void PlaceTree(Vector2 position)
    {
        // converts the position to the tilemap position
        Vector2 pos = new Vector2(treeTilemap.WorldToCell(position).x, treeTilemap.WorldToCell(position).y);
        if (placedTrees != maxTrees && !coroutines.ContainsKey(pos)) // checks to make sure the player is able to place a tree
        {
            placedTrees++; // increments the number of placed trees
            coroutines.Add(pos, DecayTree(position));
            treeTilemap.SetTile(treeTilemap.WorldToCell(position), treeTile); // sets the tile to the tree tile
            StartCoroutine(coroutines[pos]); // starts decay coroutine
        }   
    }

    public void RemoveTree(Vector2 position)
    {
        // converts the position to the tilemap position
        Vector2 pos = new Vector2(treeTilemap.WorldToCell(position).x, treeTilemap.WorldToCell(position).y);
        if (coroutines.ContainsKey(pos)) // checks to make sure there is a tree when the player is clicking
        {
            StopCoroutine(coroutines[pos]); // stops the decay coroutine
            treeTilemap.SetTile(treeTilemap.WorldToCell(position), null); // removes the tree tile
            placedTrees--; // decrements the number of placed trees
            coroutines.Remove(pos);
        }
    }
}
