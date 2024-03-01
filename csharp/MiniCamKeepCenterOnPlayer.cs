using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCamKeepCenterOnPlayer : MonoBehaviour
{
    public Camera targetCamera;
    public float scrollSpeed;
    public GameObject targetToFollow;
    public bool xzOnly;
    private float oldY;
    // Start is called before the first frame update
    void Start()
    {

        oldY = targetToFollow.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    private protected void MoveCamera()
    {
        // Adjust the camera's position based on the input
        float newX = targetToFollow.transform.position.x;
        float newZ = targetToFollow.transform.position.z;
        float newY = targetToFollow.transform.position.y;

        if (xzOnly)
        {
            newY = targetCamera.transform.position.y;
        }
        else
        {
            if (newY < oldY)
            {
                newY = newY - oldY;
            }
            newY = targetCamera.transform.position.y + newY;
        }
        oldY = newY;
        targetCamera.transform.position = new Vector3(newX, newY, newZ);
    }
}
