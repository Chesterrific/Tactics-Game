using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerMovement : MovementBase {

	//Initializes player
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (!moving)
        { 
            //Checks mouse input to see what we clicked on.
            CheckMouse();
        }
	}

    //Calls SelectableTiles function on click of the player.
    public void OnMouseDown()
    {
        //Disables function when object is under another gameobject, like UI.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!moving)
        {
            FindSelectableTiles();
        }
    }

    void CheckMouse()
    {
        //(0) is left click
        if (Input.GetMouseButtonUp(0))
        {
            //Creates a ray from the camera to the point we clicked on.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            //Check what we clicked on
            if(Physics.Raycast(ray, out hit))
            {
                //We can use other tags like enemy tag to decide what happens if we clicked on an enemy.
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        //move player to target.
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
