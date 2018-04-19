using UnityEngine;

public class CameraRotate : MonoBehaviour {

    public void RotateRight()
    {
        // Input axis to rotate around, how much to rotate, and in relation to whom
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, -90, Space.Self);
    }
	
}
