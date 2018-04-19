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
		
	}

    public void OnMouseDown()
    {
        //Disables function when object is under another gameobject, like UI.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!moving)
        {
            GetSelectableTiles();
        }
    }
}
