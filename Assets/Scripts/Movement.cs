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
    public float jumpHeight = 2.0f;
    public float jumpVelocity = 0.5f;
    private float halfHeight = 0;
    Tile currentTile; //Tile occupied by this player.

    //Player States
    Vector3 dir = new Vector3(); //Which way our player is facing.
    Vector3 velocity = new Vector3();
    Vector3 jumpTarget = new Vector3();
    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;

    //If our speed is too high, we need to set the distance comparison to be bigger
    float unstuck;

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

        unstuck = moveSpeed / 30.0f;
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
            t.FindNeighbors(jumpHeight);
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
        unstuck = moveSpeed / 25.0f;

        //As long as something's in the path, we can move.
        if (path.Count > 0)
        {
            Tile t = path.Peek();

            //Position we're moving to.
            Vector3 target = t.transform.position;

            //We add halfHeight of player to halfHeight of tile to make sure we don't move into tile and instead above it.
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= unstuck)
            {
                //Returns true if our player's position isn't the same as our calculated target y, meaning they aren't on the same level.
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    //Directs our player towards proper direction.
                    CalculateDirection(target);
                    SetHorizontalVelocity();
                }
                //Actual movement
                transform.forward = dir;
                transform.position += velocity * Time.deltaTime;
                //transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
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

            //Set new tile as our player's current tile and set tile as occupied.
            GetCurrentTile();
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

    void CalculateDirection(Vector3 target)
    {
        dir = target - transform.position;
        dir.Normalize();
    }
    
    void SetHorizontalVelocity()
    {
        velocity = dir * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }
     
    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;

        target.y = transform.position.y;

        CalculateDirection(target);

        //Falling down
        if (transform.position.y > targetY)
        {
            //We first move to edge, and therefore we aren't falling yet.
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            //We add half the distance we need to travel to our current position, giving us the edge of the current tile we're on.
            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        //Jumping up
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            //How fast we move forward in the jump up animation.
            velocity = dir * 0.5f;

            float difference = targetY - transform.position.y;

            //Initial "jump" speed.
            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target)
    {
        //Falling Speed
        velocity += Physics.gravity * 2f * Time.deltaTime ;

        if (transform.position.y < target.y)
        {
            fallingDown = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        //Strength of gravity on our player.
        velocity += Physics.gravity * 0.8f * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= unstuck) 
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            //Horizontal Speed during jump.
            velocity /= moveSpeed * 1.2f;
            velocity.y = 1.5f;
        }
    }


    /* Doesn't compute distance of tile from player correctly.
     * 
     * 
     * 
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
   */
}
