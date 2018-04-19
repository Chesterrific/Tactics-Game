using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //Move to its own script later.
    [Header("Player Stats")]
    public bool moving = false;
    public int move = 3;
    public float moveSpeed = 10.0f;

    //To hold all the tiles currently in the scene.
    GameObject[] tiles;

    Tile currentTile;

	//To be used by player after it has been initialized onto field.
	protected void Init()
    {
        //Populates tiles array with all tile objects currently in scene.
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        GetCurrentTile();
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

    //Helper function to set up recursion for later.
    public void GetSelectableTiles()
    {
        //Takes all tiles and finds their adjacent tiles.
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
        Tile workingTile = currentTile;
        //Begin recursion.
        GetTiles(workingTile, 0);
    }

    /* Grabs adjacent tiles of adjacent tiles until counter equals move.
     * This function will keep going in one direction until counter matches move,
     * it then backtracks to hit all other directions until it exhausts all possibilities.
     */
    public void GetTiles(Tile tile, int counter)
    {
        if(counter == move)
        {
            return;
        }
        for (int i = 0; i < tile.adjacentTiles.Count; i++)
        {
            if (tile.adjacentTiles[i].walkable && !tile.adjacentTiles[i].current && !tile.adjacentTiles[i].occupied)
            {
                tile.adjacentTiles[i].selectable = true;
            }
            GetTiles(tile.adjacentTiles[i], counter + 1);
        }
    }
}
