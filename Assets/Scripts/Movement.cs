using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //To hold all the tiles currently in the scene.
    GameObject[] tiles;

    //Move to its own script later.
    [Header("Player Stats")]
    public bool moving = false;
    public int move = 3;
    public float moveSpeed = 10.0f;
    private float halfHeight = 0;
    Tile currentTile; //Tile occupied by this player.

    //List that holds all currently selectable tiles of this player.
    public List<Tile> selectableTiles = new List<Tile>();

    //Path to follow
    public Stack<Tile> path = new Stack<Tile>();

	//To be used by player after it has been initialized onto field.
	protected void Init()
    {
        //Populates tiles array with all tile objects currently in scene.
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        //Calculates halfHeight of player.
        halfHeight = GetComponent<Collider>().bounds.extents.y;
	}
	
    //To be used to find currentTile.
	public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
    }

    //Should return tile right below player.
    public Tile GetTargetTile(GameObject target){
        RaycastHit hit;
        Tile tile = null;

        //If our raycast hits something 1 unit below our player, we return that collider's tile component.
        if (Physics.Raycast(target.transform.position, Vector3.down, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        tile.occupied = true;
        tile.current = true;
        return tile;
    }

    //Takes all tiles and finds their adjacent tiles
    public void ComputeAdjacentTiles()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
    }
    public void FindSelectableTiles()
    {
        //We compute the adjacency list of all the tiles in the game.
        ComputeAdjacentTiles();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

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
            //We process all the tiles within our range.
            if (t.distanceFromOrigin < move)
            {
                foreach (Tile tile in t.adjacentTiles)
                {
                    //If a tile has been visited before, we don't reprocess it.
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distanceFromOrigin = 1 + t.distanceFromOrigin;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
    
    public void MoveToTile(Tile target)
    {
        //Clear old path info.
        path.Clear();

        target.target = true;
        moving = true;

        Tile next = target;
        while(next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        Vector3 dir = new Vector3();

        //If our speed is too high, we need to set the distance comparison to be bigger
        float unstuck = 0.3f;
        if(moveSpeed > 35f)
        {
            unstuck = 0.6f;
        }

        //As long as something's in the path, we can move.
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //We add halfHeight of player to halfHeight of tile to make sure we don't move into tile and instead above it.
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= unstuck)
            {
                //Directs our player towards proper direction
                dir = target - transform.position;

                //Actual movement
                transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                //We've reached the tile
                transform.position = target;
                path.Pop();
            }
        } 
        else
        {
            RemoveSelectableTiles();
            moving = false;
        }
    }

protected void RemoveSelectableTiles()
    {
        //After we're finished moving, our old current tile is reset.
        if (currentTile != null)
        {
            currentTile.occupied = false;
            currentTile.current = false;
            currentTile = null;
        }

        //We reset the selectableTiles array after we move.
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();

    }

      
    /* Doesn't compute distance correctly.
    //Helper function to set up recursion for later.
    public void GetSelectableTiles()
    {
        ComputeAdjacentTiles();
        GetCurrentTile();

        //Set up recursion.
        Tile workingTile = currentTile;
        int distance = 0;

        //Begin recursion.
        GetTiles(workingTile, distance);
    }

    /* Grabs adjacent tiles of adjacent tiles until counter equals move.
     * This function will keep going in one direction until counter matches move,
     * it then backtracks to hit all other directions until it exhausts all possibilities.
     */
    /*
   public void GetTiles(Tile tile, int distance)
   {
       tile.distanceFromOrigin = distance;
       if (distance == move)
       {
           return;
       }
       for (int i = 0; i < tile.adjacentTiles.Count; i++)
       {
           if (tile.adjacentTiles[i].walkable && !tile.adjacentTiles[i].current && !tile.adjacentTiles[i].occupied && !tile.adjacentTiles[i].selectable)
           {
               //tile.adjacentTiles[i].distanceFromOrigin = distance + 1;
               tile.adjacentTiles[i].parent = tile;
               tile.adjacentTiles[i].selectable = true;
           }
           GetTiles(tile.adjacentTiles[i], distance + 1);
       }
   }


   public void SetTargetLocation(Tile target)
   {

   }*/
}
