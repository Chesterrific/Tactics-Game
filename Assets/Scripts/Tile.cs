using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    [Header("Tile States")]
    public bool selectable = false;
    public bool current = false;
    public bool target = false;
    public bool walkable = true;
    public bool occupied = false;

    //[HideInInspector] //For movement selection
    //public bool visited = false;

    [Header("Tile Color Options")]
    public Color cSelectable = Color.blue;
    public Color cCurrent = Color.red;
    public Color cTarget = Color.yellow;

    //Tile specific items
    private Renderer rend;
    private Color startColor;

    //List of tiles that are adjacent to this one, will use for movement calculations.
    public List<Tile> adjacentTiles = new List<Tile>();


    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        //FindNeighbors();
    }

    private void Update()
    {
        if (selectable)
        {
            rend.material.color = cSelectable;
        }
        else if (current)
        {
            rend.material.color = cCurrent;
        }
        else if (target)
        {
            rend.material.color = cTarget;
        }
        else
        {
            //Returns tile to normal color after all settings and movement.
            rend.material.color = startColor;
        }

    }

    //Will check all surronding tiles in each direction for another tile.
    public void FindNeighbors()
    {
        Reset();
        CheckTile(Vector3.forward); 
        CheckTile(Vector3.back);
        CheckTile(Vector3.left);
        CheckTile(Vector3.right);
    }

    //Will add onto adjacenctTiles list
     public void CheckTile(Vector3 direction)
    {
        Vector3 halfTileSize = rend.bounds.extents;

        /* OverlapBox requires a Vector3 position to put the center of the box,
         * and half the size of the box in each direction. So puting the box 1 unit
         * in our desided direction and making the halfextent 1/4 of our tile size should 
         * place the box in the middle of a tile and about .5*.5*.5. Ensuring it
         * collides with the given neighbor tile.
         */

        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfTileSize/2);

        //Grab each item in colliders array, basically what collided with our box from above.
        foreach(Collider item in colliders)
        {
            //Grab the tile component of the item
            Tile tile = item.GetComponent<Tile>();

            //If the tile is not null, is walkable, and is not occupied, it is added to the list of adjacent tiles.
            //May change this later for targeting and etc.
            if (tile != null && tile.walkable && !tile.occupied)
            {
                adjacentTiles.Add(tile);
            }
        }
    }

    public void Reset()
    {
        adjacentTiles.Clear();
        current = false;
        target = false;
        selectable = false;
    }
}
