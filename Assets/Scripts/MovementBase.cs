using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour {

    //List to hold our adjacent tiles.
    List<Tile> selectableTiles = new List<Tile>();

    //Holds all the tiles in the game.
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    public bool moving = false;
    public int move = 5;
    public float moveSpeed = 2.0f;
    public float jumpHeight = 2.0f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    protected void Init()
    {
        //We can set this to update if we have tiles that dissapear or appear.
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    //Returns tile under player
    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        //If raycast hits something below player, we set that collider to the hit variable.
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            //We grab that collider's tile component and return that tile as our current tile.
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;

    }

    public void ComputeAdjacencyLists()
    {
        //This code would be for something where the stage changes, i.e. different tiles are loaded and unloaded.
        //tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            //We grab the tile component of the GameObject tile
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight);
        }
    }

    public void FindSelectableTiles()
    {
        //We compute the adjacency list of all the tiles in the game.
        ComputeAdjacencyLists();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        //First step in BFS is to add current tile.
        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ??  leave as null 

        //While the queue is not empty, we can still move.
        while (process.Count > 0)
        {
            //Handle first tile in queue and put it into variable t.
            Tile t = process.Dequeue();

            //Add t into selectableTiles and turn it into c_selectable color.
            selectableTiles.Add(t);
            t.selectable = true;

            //While the distance we want to move doesn't exceed how far away we can move.
            //We process all the tiles within our range
            if (t.distance < move)
            {
                //We add each tile in t's adjacency list into the queue for bfs processing.
                foreach (Tile tile in t.adjacencyList)
                {
                    //If a tile has been visited before, we don't reprocess it in the BFS
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }
}
