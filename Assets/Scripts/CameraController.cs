using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //Holds different locations we want.
    public Transform[] views;
    public float transitionSpeed;

    private Transform currentView;
    private int viewCounter;


    private void Start()
    {
        viewCounter = 0;
        currentView = views[viewCounter];
        
    }

    public void RotateLeft()
    {
        //transform.Rotate(axis, degrees to rotate, rotation relative to what)
        //transform.Rotate(Vector3.up, 90, Space.Self);

        
        viewCounter++;
        //Reset viewCounter to 0 the start if we exceed number of views
        if(viewCounter > views.Length-1)
        {
            viewCounter = 0;
        }
        currentView = views[viewCounter];
        
        
    }

    public void RotateRight()
    {
        //transform.Rotate(axis, degrees to rotate, rotation relative to what)
        //transform.Rotate(Vector3.up, -90, Space.Self);

        
        viewCounter--;
        //Reset viewCounter to end if we leave the bounds of view[].
        if(viewCounter < 0)
        {
            viewCounter = views.Length-1;
        }
        currentView = views[viewCounter];
        
    }


    private void LateUpdate()
    {
        //Lerp to position, smooth transform to position.
        //transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, currentView.rotation, Time.deltaTime * transitionSpeed);
    }
}
