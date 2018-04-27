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
    public bool pointOfInterest = false;
    

    [Header("Tile Color Options")]
    public Color cSelectable = Color.blue;
    public Color cCurrent = Color.red;
    public Color cTarget = Color.yellow;


    [Header("Movement Variables")]
    public bool visited = false;
    public Tile parent = null;
    public int distanceFromOrigin;

    [Header("Camera Angles")]
    public Transform[] tileViews;
    public Vector3 camOffset = new Vector3(6, -7.5f, 6f);
    private bool needToResetCam = false;


    //Tile specific items
    private Renderer rend;
    private Color startColor;
    private CameraController cam;
     

    //List of tiles that are adjacent to this one, will use for movement calculations.
    public List<Tile> adjacentTiles = new List<Tile>();


    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        cam = CameraController.cam;
        distanceFromOrigin = 0;
        //Finds all tile's neighbors at the start of the game.
        //FindNeighbors();
    }

    private void Update()
    {

        if (target)
        {
            rend.material.color = cTarget;
        }
        else if (current)
        {
            rend.material.color = cCurrent;
        }
        else if (selectable)
        {
            rend.material.color = cSelectable;
        }
        else
        {
            //Returns tile to normal color after all settings and movement.
            rend.material.color = startColor;
        }
        if (pointOfInterest)
        {
            cam.FocusOnPOI(this, camOffset);
            needToResetCam = true;
        }
        else if (!pointOfInterest && needToResetCam)
        {
            cam.ResetCamera();
            needToResetCam = false;
        }

    }

    /* Will check all surronding tiles in each direction for another tile.
     * Jump Height is for finding possible tiles to move to based on player's jumpHeight stat.
     */
    public void FindNeighbors(float jumpHeight)
    {
        Reset();
        CheckTile(Vector3.forward, jumpHeight); 
        CheckTile(Vector3.back, jumpHeight);
        CheckTile(Vector3.left, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
    }

    //Will add onto adjacenctTiles list
     public void CheckTile(Vector3 direction, float jumpHeight)
    {

        //transform.GetComponent<Collider>().bounds.extents.x)/2.0f grabs the tile's halfextend in the x direction and divides it by two for the overlap box function later.

        Vector3 halfTileSize = new Vector3((transform.GetComponent<Collider>().bounds.extents.x)/2.0f, jumpHeight, (transform.GetComponent<Collider>().bounds.extents.z)/2.0f);
        
        /* OverlapBox requires a Vector3 position to put the center of the box,
         * and half the size of the box in each direction. So puting the box 1 unit
         * towards our desided direction and making the halfextent 1/4 of our tile size should 
         * place a .5 * jumpheight * .5 sized collision box in the middle of the tile. Ensuring it
         * only collides with the given neighbor tile we want.
         */

        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfTileSize);

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

        selectable = false;
        current = false;
        target = false;
        pointOfInterest = false;
        //occupied = false;

        parent = null;
        visited = false;
        distanceFromOrigin = 0;
    }
}
