using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    //Neighbors of each tile
    public List<Tile> adjacencyList = new List<Tile>();

    //For BFS
    public bool visited = false;
    public Tile parent = null;
    public int distance;

    [Header("Tile Options")]
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    [Header("Tile Color Options")]
    public Color c_current;
    public Color c_target = Color.yellow;
    public Color c_selectable = Color.blue;

    //Tile specific items
    private Renderer rend;
    private Color startColor;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        //adjacenyList = new List<Tile>();
        distance = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (current)
        {
            rend.material.color = c_current;
        }
        else if (target)
        {
            rend.material.color = c_target;
        }
        else if (selectable)
        {
            rend.material.color = c_selectable;
        }
        else
        {
            rend.material.color = startColor;
        }
	}

    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;
    }

    //Checks each tile around current for neighbors.
    //This function will populate the adjacency list of each individual tile in the game with a max of 4 tiles each.
    public void FindNeighbors(float jumpHeight)
    {
        Reset();
        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(Vector3.back, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(Vector3.left, jumpHeight);
    }

    //This function constructs the adjacencyList.
    public void CheckTile(Vector3 direction, float jumpHeight)
    {
        //1+jumpHeight/2 will determine which tiles we can move across based on our jumpheight.
        //This vector extends about .25 units to the x and z and a bit over the y to find tiles.
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);

        //Will determine if a tile is present next to current tile
        //OverlapBox finds all colliders of objects that touch or overlap with the given box dimensions. In this case the halfExtents.
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        //Grabs each collider returned from code above.
        foreach (Collider item in colliders)
        {
            //Grabs tile component from the colliders.
            Tile tile = item.GetComponent<Tile>();

            //If the given tile is not null and the tile is walkable.
            if(tile != null && tile.walkable)
            {
                RaycastHit hit;

                //This call returns true if it hits something above the tile, namely a player in our case.
                /* Maybe replace with something that tells us if a tile has someone on it by itself instead of using physics?
                 * Something like bool occupied and set it to true or false each time something moves there?
                 */
                //If there is nothing 1 unit above the tile currently being worked on, it is added as an adjacent tile to the list.
                if(!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    //Adjacent tile is found and added to list of adjacent tiles
                    adjacencyList.Add(tile);
                }  
            }
        }
    }
}
