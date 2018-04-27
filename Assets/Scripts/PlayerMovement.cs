using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement {

	//Player will find its current tile at time of initialization.
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {

        /*if (!turn)
        {
            return;
        }*/

        if (!thisPlayer.moving)
        {
            CheckMouse();
        }
        else
        {
            Move();
        }
	}

    public void OnMouseDown()
    {
        //Disables function when object is under another gameobject, like UI.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!thisPlayer.moving)
        {
            FindSelectableTiles();
        }
    }

    void CheckMouse()
    {
        //Disables function when object is under another gameobject, like UI.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButton(0))
        {
            //Creates a ray from camera to mouse position.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            //If our ray hit something, output it to hit.
            if(Physics.Raycast(ray, out hit))
            {
                //Check if what we hit was a Tile.
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
