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
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        currentTile = GetCurrentTile();
        currentTile.current = true;
	}
	
    //Should return tile right below player.
	public Tile GetCurrentTile()
    {
        RaycastHit hit;
        Tile tile = null;

        //If our raycast hits something 1 unit below our player, we return that collider's tile component.
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        tile.occupied = true;
        
        return tile;
    }

    public void GetSelectableTiles()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
        GetCurrentTile();
        Tile workingTile = currentTile;
        GetTiles(workingTile, 0);
    }

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
