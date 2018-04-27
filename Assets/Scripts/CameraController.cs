using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController cam;

    [Header("Camera Angles and Views")]
    public Transform[] views;
    public Transform startingCameraPosition;
    public Transform startingCameraZoom;
    public Camera mainCamera;

    private Transform currentView;
    private int viewCounter;

    [Header("Camera Options")] //Can be set per map if we get that far.
    public float panSpeed = 15f;
    public float scrollSpeed = 20f;
    public float minX = -15f;
    public float maxX = 30f;
    public float minZ = -15f;
    public float maxZ = 30f;
    public float maxZoomIn = 3.5f;
    public float maxZoomOut = 17.5f;
    public float transitionSpeed = 5f;
    public float cameraResetSpeed = .5f;

    //To allow access of camera variables to everything in the project
    private void Awake()
    {
        if(cam != null)
        {
            return;
        }
        cam = this;
    }

    private void Start()
    {
        viewCounter = 0;
        currentView = views[viewCounter];
        
    }

    void Update()
    {
        /* Turn off camera control on game over, implement later
        if (GameManager.GameIsOver)
        {
            this.enabled = false;
            return;
        }*/

        //Camera controller's position.
        Vector3 pos = transform.position;

        //up 
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime); 
        }

        //down
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime); 
        }

        //left
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime);
        }

        //right
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime); 
        }

        //Middle Scroll Wheel click or if camera goes way out of bounds
        if (Input.GetMouseButtonDown(2) || pos.x > maxX || pos.z > maxZ || pos.x < minX || pos.z < minZ)
        {
            ResetCamera();
        }

        //mouse scrolling
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        //Store mainCamera position.
        Vector3 zoom = mainCamera.transform.position;

        if (scroll > 0f && zoom.y > maxZoomIn)
        {
            mainCamera.transform.Translate(Vector3.forward * scrollSpeed * Time.deltaTime, Space.Self);
            
        }

        else if (scroll < 0f && zoom.y < maxZoomOut)
        {
            mainCamera.transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime, Space.Self);
           
        }
        
    }

    //Resets camera coordinates to given startingCameraPosition.
    public void ResetCamera()
    {
        //viewCounter = 0;
        currentView = views[viewCounter];
        StartCoroutine(Transition());
        
    }

    IEnumerator Transition()
    {
        float t = 0.0f;
        Vector3 currentCamPos = transform.position;
        Vector3 currentCamZoom = mainCamera.transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / cameraResetSpeed);

            //Lerps to starting positions
            transform.position = Vector3.Lerp(currentCamPos, currentView.position, t);
            mainCamera.transform.position = Vector3.Lerp(currentCamZoom, startingCameraZoom.position, t);

            yield return 0;
        }

    }

    public void RotateLeft()
    {       
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
        viewCounter--;
        //Reset viewCounter to end if we leave the bounds of view[].
        if(viewCounter < 0)
        {
            viewCounter = views.Length-1;
        }
        currentView = views[viewCounter];
       
    }

    public void FocusOnPOI(Tile tile, Vector3 offSet)
    {
        currentView = tile.tileViews[viewCounter];
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

    }

    private void LateUpdate()
    {
        //Lerp to position, smooth transform to position.
        //transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        //From start position to end position, in our case rotations.
        transform.rotation = Quaternion.Lerp(transform.rotation, currentView.rotation, Time.deltaTime * transitionSpeed);
    }
}
