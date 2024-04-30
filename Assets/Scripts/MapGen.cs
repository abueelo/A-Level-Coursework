using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    // corner grid positions of the board
    private float[] topLeft = new float[2] {-6.5f, 3.5f};
    private float[] bottomRight = new float[2] {6.5f, -3.5f};

    public Tilemap obstacleTilemap;
    public Tile treeTile;

    public GameObject collectablePrefab;
    public GameObject flagPrefab;
    public GameObject playerPrefab;
    public GameObject minotaurPrefab;

    private GameObject placedFlag;
    private GameObject placedPlayer;
    private GameObject placedMinotaur;

    void Start()
    {
        obstacleTilemap = GameObject.Find("Obstacles").GetComponent<Tilemap>(); // gets the tilemap to place obstacles onto
        GenerateMap(); // places trees
        PlaceFlag(); // places flag
        SpawnPlayer(); // spawns player
        SpawnSpawnables(); // spawns key and random items
        SpawnMinotaur(); // spawns minotaur
    }

    private void GenerateMap()
    {
        // Place trees
        for (float x = topLeft[0]; x <= bottomRight[0]; x++) // loops through the x values in the board
        {
            for (float y = topLeft[1]; y >= bottomRight[1]; y--) // loops through the y values in the board
            {
                if (Random.Range(0, 100) >= 85) // random chance to place a tree
                {
                    Debug.Log("placed tree at: " + x + ", " + y);
                    obstacleTilemap.SetTile(obstacleTilemap.WorldToCell(new Vector3(x, y, 0f)), treeTile); // places the tree
                }
            }
        }
    }

    private void SpawnSpawnables()
    {

        // Place key
        bool placedKey = false; // flag to make sure a key is placed
        while (!placedKey)
        {
            for (float x = topLeft[0]; x <= bottomRight[0]; x++) // loops through the x values in the board
            {
                for (float y = topLeft[1]; y >= bottomRight[1]; y--) // loops through the y values in the board
                {
                    if(obstacleTilemap.GetTile(obstacleTilemap.WorldToCell(new Vector3(x, y, 0f))) == null) // checks to make sure the tile is not an obstacle
                    {
                        if (Random.Range(0, 100) >= 85) // random chance to place a key
                        {
                            Debug.Log("placed key at: " + x + ", " + y);
                            Instantiate(collectablePrefab, new Vector3(x, y, 0f), Quaternion.identity); // spawns key at position
                            placedKey = true; // sets flag to true
                            break;
                        }
                    }
                }
                if(placedKey){break;}
            }
        }

        // Place at least i random items
        int i = 3;
        int count = 0; 
        while (count < i) // loops until i items are placed
        {
            for (float x = topLeft[0]; x <= bottomRight[0]; x++) // loops through the x values in the board
            {
                for (float y = topLeft[1]; y >= bottomRight[1]; y--) // loops through the y values in the board
                {
                    // checks to make sure the tile is empty
                    if(obstacleTilemap.GetTile(obstacleTilemap.WorldToCell(new Vector3(x, y, 0f))) == null && placedFlag.transform.position != new Vector3(x, y, 0f) && placedPlayer.transform.position != new Vector3(x, y, 0f))
                    {
                        if (Random.Range(0, 100) >= 90 && count < i) // random chance to place an item
                        {
                            Debug.Log("placed item at: " + x + ", " + y);
                            // spawns item at position
                            Collectable collectable = Instantiate(collectablePrefab, new Vector3(x, y, 0f), Quaternion.identity).GetComponent<Collectable>();
                            int rnd = Random.Range(0, 100); // random number to determine item type
                            switch (rnd)
                            {
                                case int n when (n < 10): // if the number is less than 10, place a sword
                                    collectable.type = Collectable.CollectableType.Sword;
                                    break;
                                case int n when (n > 10 && n < 20): // if the number is between 10 and 20, place an axe
                                    collectable.type = Collectable.CollectableType.Axe;
                                    break;
                                case int n when (n > 20 && n < 25): // if the number is between 20 and 25, place a path item
                                    collectable.type = Collectable.CollectableType.PathItem;
                                    break;
                                default: // else place a potion
                                    collectable.type = Collectable.CollectableType.Potion;
                                    break;
                            }
                            count++;
                        }
                    }
                }
            }
        }
    }

    private void PlaceFlag()
    {
        bool placed = false; // flag to make sure a flag is placed
        while(!placed)
        {
            for (float x = topLeft[0]; x <= bottomRight[0]; x++) // loops through the x values in the board
            {
                for (float y = topLeft[1]; y >= bottomRight[1]; y--) // loops through the y values in the board
                {
                    if(obstacleTilemap.GetTile(obstacleTilemap.WorldToCell(new Vector3(x, y, 0f))) == null) // checks to make sure the tile is free to place the flag
                    {
                        if (Random.Range(0, 100) >= 85) // random chance to place a flag
                        {
                            Debug.Log("placed flag at: " + x + ", " + y);
                            placedFlag = Instantiate(flagPrefab, new Vector3(x, y, 0f), Quaternion.identity); // spawns flag at position
                            placed = true; // sets flag to true
                            break;
                        }
                    }
                }
                if(placed){break;}
            }
        }
    }

    private void SpawnPlayer()
    {
        bool placed = false; // flag to make sure a player is placed
        while(!placed)
        {
            for (float x = topLeft[0]; x <= bottomRight[0]; x++) // loops through the x values in the board
            {
                for (float y = topLeft[1]; y >= bottomRight[1]; y--) // loops through the y values in the board
                {
                    // checks to make sure the tile is free to place the player
                    if(obstacleTilemap.GetTile(obstacleTilemap.WorldToCell(new Vector3(x, y, 0f))) == null && placedFlag.transform.position != new Vector3(x, y, 0f))
                    {
                        if (Random.Range(0, 100) >= 80) // random chance to place the player
                        {
                            Debug.Log("placed player at: " + x + ", " + y);
                            placedPlayer = Instantiate(playerPrefab, new Vector3(x, y, 0f), Quaternion.identity); // spawns player at position
                            placed = true;
                            break;
                        }
                    }
                }
                if(placed){break;}
            }
        }
    }
    
    private void SpawnMinotaur()
    {
        bool placed = false; // flag to make sure a minotaur is placed
        while(!placed)
        {
            for (float x = topLeft[0]; x <= bottomRight[0]; x++) // loops through the x values in the board
            {
                for (float y = topLeft[1]; y >= bottomRight[1]; y--) // loops through the y values in the board
                {
                    // checks to make sure the tile is free to place the minotaur
                    if(obstacleTilemap.GetTile(obstacleTilemap.WorldToCell(new Vector3(x, y, 0f))) == null && placedFlag.transform.position != new Vector3(x, y, 0f) && placedPlayer.transform.position != new Vector3(x, y, 0f))
                    {
                        if (Random.Range(0, 100) >= 80) // random chance to place the minotaur
                        {
                            Debug.Log("placed player at: " + x + ", " + y);
                            placedMinotaur = Instantiate(minotaurPrefab, new Vector3(x, y, 0f), Quaternion.identity); // spawns minotaur at position
                            placedMinotaur.GetComponent<Minotaur>().player = placedPlayer.GetComponent<Player>(); // sets the player for the minotaur to chase after
                            placed = true;
                            break;
                        }
                    }
                }
                if(placed){break;}
            }
        }
    }
}
