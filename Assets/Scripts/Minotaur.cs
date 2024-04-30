using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Minotaur : MonoBehaviour
{
    public Player player;
    private float nextMoveTime;

    private Tilemap obstacleTilemap;
    private Tilemap waterTilemap;
    private GameObject flag;

    private Stack<Vector3> pathToPlayer = new Stack<Vector3>();

    public Stack<Vector3> Pathfind()
    {
        // defining queue of nodes and the visited hashset
        Queue<Node> queue = new Queue<Node>();
        HashSet<Vector3> visited = new HashSet<Vector3>();

        Node startNode = new Node(transform.position, null); // Create the starting node
        queue.Enqueue(startNode); // Add the starting node to the queue
        visited.Add(startNode.position); // Mark the starting node as visited

        while (queue.Count > 0) // While there are nodes in the queue
        {
            Node currentNode = queue.Dequeue(); // Get the next node from the queue

            if (currentNode.position == player.transform.position) // check to see if this is the player
            {
                // Path found, construct and return the path
                Stack<Vector3> path = new Stack<Vector3>();
                Node node = currentNode;
                while (node != null)
                {
                    path.Push(node.position);
                    node = node.parent;
                }
                path.Pop(); // Remove the start position
                return path;
            }

            // Add neighbouring nodes to the queue
            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!visited.Contains(neighbour.position))
                {
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour.position);
                }
            }
        }

        // No path found
        return null;
    }
    
    private List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Check Left
        if (isValidPosition(new Vector3(node.position.x - 1,node.position.y,0)))
        {
            neighbours.Add(new Node(new Vector3(node.position.x - 1, node.position.y, 0), node));
        }

        // Check Right
        if (isValidPosition(new Vector3(node.position.x + 1,node.position.y,0)))
        {
            neighbours.Add(new Node(new Vector3(node.position.x + 1, node.position.y, 0), node));
        }

        // Check Up
        if (isValidPosition(new Vector3(node.position.x,node.position.y+1,0)))
        {
            neighbours.Add(new Node(new Vector3(node.position.x, node.position.y + 1, 0), node));
        }

        // Check Down
        if (isValidPosition(new Vector3(node.position.x,node.position.y-1,0)))
        {
            neighbours.Add(new Node(new Vector3(node.position.x, node.position.y - 1, 0), node));
        }

        return neighbours;
    }

    private bool isValidPosition(Vector3 position)
    {
        Vector3Int gridPosition = obstacleTilemap.WorldToCell(position);
        if (obstacleTilemap.GetTile(gridPosition) || waterTilemap.GetTile(gridPosition) || flag.transform.position == position)
        {
            return false;
        }
        return true;
    }

    void Start()
    {
        // Get components from the scene
        flag = GameObject.Find("Flag(Clone)");
        obstacleTilemap = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        waterTilemap = GameObject.Find("Water").GetComponent<Tilemap>();

        nextMoveTime = Time.time+.5f;
    }

    void LateStart()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        
        if (Time.time > nextMoveTime) // Check to see if minotaur can move
        {
            pathToPlayer = Pathfind(); // Find the path to the player
            nextMoveTime = Time.time+.5f; // Move every 0.5 seconds
            if (pathToPlayer != null && pathToPlayer.Count > 0) // Check to see if there is a path to the player
            {
                Vector3 value = pathToPlayer.Pop(); // Get the next position in the path
                transform.position = value; // move there
            }
            if (transform.position == player.transform.position) // if the minotaur ontop of the player
            {
                Globals.health--; // hurt them
                Debug.Log("ouch");
            }
        }
    }

    public void kill()
    {
        Destroy(this.gameObject); // destroy ourselves
    }
}
