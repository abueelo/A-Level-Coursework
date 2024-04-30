using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    public float moveSpeed = 10;
    public PlayerUI playerUI;
    public WDUIController WDUIcontroller;
    public Transform flagPosition;
    public Tile padlock;
    public Tile deniedPadlock;
    
    private bool canMove = true;
    private float gridSize = 1f;
    private Tilemap obstacleTilemap;
    private Tilemap waterTilemap;
    private Tilemap padlockTilemap;

    private Transform flag;

    // placing trees
    public TreeController treeController;

    void Start()
    {
        // Gets all the required components from the scene
        obstacleTilemap = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        waterTilemap = GameObject.Find("Water").GetComponent<Tilemap>();
        padlockTilemap = GameObject.Find("Padlock").GetComponent<Tilemap>();
        flag = GameObject.Find("Flag(Clone)").GetComponent<Transform>();
        padlockTilemap.SetTile(padlockTilemap.WorldToCell(flag.position), padlock); // add padlock to the flag to show that it is locked
        treeController = GameObject.Find("TreeController").GetComponent<TreeController>();

        WDUIcontroller = gameObject.GetComponentInChildren<WDUIController>();
        WDUIcontroller.gameObject.SetActive(false);
    }

    public void Update()
    {
        Move();


        if (Input.GetKeyDown(KeyCode.Mouse0)) // left click input detection
        {
            treeController.PlaceTree(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) // right click input detection
        {
            treeController.RemoveTree(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }

        if(Globals.health <= 0) // check to see if the player has died
        {
            WDUIcontroller.Die();
            canMove = false;
        }
    }

    private void Move()
    {
        if (canMove)
        {
            MoveHorizontal();
            MoveVertical();
        }
    }

    private void MoveVertical()
    { 
        Vector2 destination = transform.position;
        if (Input.GetKeyDown(KeyCode.W)) // check for positive vertical input
        {
            destination = new Vector2(transform.position.x,transform.position.y+gridSize); // move up one grid space
        }
        else if (Input.GetKeyDown(KeyCode.S)) // check for negative vertical input
        {
            destination = new Vector2(transform.position.x,transform.position.y-gridSize); // move down one grid space
        }

        if (CheckForCollision(destination) == false) // check if we can move to the destination
        {
            transform.position = destination; // make the move
        }
    }

    private void MoveHorizontal()
    {
        Vector2 destination = transform.position;
        if (Input.GetKeyDown(KeyCode.A)) // check for negative horizontal input
        {
            destination = new Vector2(transform.position.x-gridSize,transform.position.y); // move left one grid space

        }
        else if (Input.GetKeyDown(KeyCode.D)) // check for positive horizontal input
        {
            destination = new Vector2(transform.position.x+gridSize,transform.position.y); // move right one grid space
        }

        if (CheckForCollision(destination) == false) // check if we can move to the destination
        {
            transform.position = destination; // make the move
        }
    }

    private bool CheckForCollision(Vector2 location)
    {
        Vector3Int gridPosition = obstacleTilemap.WorldToCell(location); // convert the location to a grid position
        // check if there is something that we shouldnt collide with at the location we are trying to move to
        if (obstacleTilemap.GetTile(gridPosition) || waterTilemap.GetTile(gridPosition))
        {
            Debug.Log("collided with obstacle");
            return true;
        }
        if (CheckForFlag(location)) 
        {
            return true;
        }
        return false;
    }

    public void CollectCollectable(Collectable.CollectableType type)
    {
        switch(type)
        {
            case Collectable.CollectableType.Potion: // if type is potion
                if(Globals.health != Globals.maxHealth)
                {
                    Globals.health++; // increase health if not at max
                    Debug.Log("Potion Collected, health increased");
                }else
                {
                    Globals.inventory.Add(type); // add potion to inventory if health is full
                    Debug.Log("Potion Collected, health full");
                }
                break;
            case Collectable.CollectableType.PathItem: // if type is path item
                Globals.canSeePath = true; // sets the ability to toggle path feature to true
                Debug.Log("Path Item Collected");
                break;
            default:
                Globals.inventory.Add(type); // else add the item type to the inventory
                Debug.Log(type + " Collected");
                break;
        }
    }

    IEnumerator PadLockAnim(Vector2 location) // animates padlock for when the player tries to unlock the flag without a key
    {
        padlockTilemap.SetTile(padlockTilemap.WorldToCell(location), deniedPadlock); // set the tile to the denied padlock
        yield return new WaitForSeconds(0.5f); // wait 0.5 seconds
        padlockTilemap.SetTile(padlockTilemap.WorldToCell(location), padlock); // set the tile back to the padlock
    }

    IEnumerator DeniedAnim(Vector2 location)
    {
        padlockTilemap.SetTile(padlockTilemap.WorldToCell(location), deniedPadlock); // set the tile to the denied padlock
        yield return new WaitForSeconds(0.5f); // wait 0.5 seconds
        padlockTilemap.SetTile(padlockTilemap.WorldToCell(location), null); // removes the padlock
    }

    private bool CheckForFlag(Vector2 location)
    {
        if (new Vector3(location.x, location.y, 0) == flag.position) // check if there is a flag at the location
        {
            if (Globals.inventory.Contains(Collectable.CollectableType.Key))
            {
                // if the player has a key then they win the level
                Debug.Log("You Win!");
                Globals.wins++;
                canMove = false;
                padlockTilemap.SetTile(padlockTilemap.WorldToCell(location), null); // removes the padlock from the flag
                WDUIcontroller.Win(); // shows the win screen
                GameObject.Find("Minotaur(Clone)").GetComponent<Minotaur>().kill(); // kills the minotaur
                Globals.inventory.Remove(Collectable.CollectableType.Key); // removes the key from the inventory
                return false; // allow the player to move onto the flag
            }
            else
            {
                Debug.Log("You need a key to unlock the flag");
                return true;
            }
        }
        return false;
    }
}
