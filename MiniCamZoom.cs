using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCamZoom : MonoBehaviour
//{
//    public Camera targetCamera;
//    public float scrollSpeed = 5.0f;

//    private Vector2 lastTapPos;
//    private bool isDragging = false;
//    private float lastTapTime;
//    private bool isEngaged = false;
//    void Update()
//    {
//        // Check for double-click or tap
//        if (Input.GetMouseButtonDown(0) && IsDoubleClick() || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//        {
//            if (isEngaged) { isEngaged = false; return; }
//            lastTapPos = Input.mousePosition;
//            lastTapTime = Time.time;
//        }

//        // Check for mouse wheel or touch drag
//        if (Input.GetAxis("Mouse ScrollWheel") != 0 || isDragging)
//        {
//            float deltaY = Input.GetAxis("Mouse ScrollWheel");
//            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
//            {
//                deltaY = Input.GetTouch(0).deltaPosition.y / Screen.height;
//            }
//            if (isEngaged)
//            {
//                MoveCamera(deltaY);
//            }
//            if (!isEngaged) { isEngaged = true; }

//        }
//    }

//    bool IsDoubleClick()
//    {
//    float doubleClickDistanceThreshold = 10f;

//    float doubleClickTimeThreshold = 1f;
//        if (Time.time - lastTapTime < doubleClickTimeThreshold)
//        {
//            return Vector2.Distance(lastTapPos, Input.mousePosition) < doubleClickDistanceThreshold;
//        }
//        else
//        {
//            lastTapTime = Time.time;
//            return false;
//        }
//    }

//    void MoveCamera(float deltaY)
//    {
//        // Adjust the camera's position based on the input
//        float newY = targetCamera.transform.position.y + deltaY * scrollSpeed;
//        float newX = targetCamera.transform.position.x;
//        float newZ = targetCamera.transform.position.z;
//        targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, Mathf.Clamp(newY, newY, newY), targetCamera.transform.position.z);
//    }
//}


{
    public Camera targetCamera;
    public float scrollSpeed = 5.0f;

    private Vector2 lastTapPos;
    private bool isDragging = false;
    private bool isEngaged = false;
    private float lastTapTime;

    // Define the screen portion for engagement (e.g., bottom right corner)
    private Rect engagementBounds = new Rect(0.7f, 0.3f, 0.3f, 0.5f);

    private void Update()
    {
        // Check for engagement within specified screen portion
        if (!isEngaged && IsEngagementAreaClicked())
        {
            lastTapPos = Input.mousePosition;
            EngageCameraControl();
        }

        // Check for disengagement on right mouse click
        if (isEngaged && Input.GetMouseButtonDown(1))
        {
            DisengageCameraControl();
        }

        // Continue only if engaged
        if (isEngaged)
        {
            // Check for mouse wheel or touch drag
            if (Input.GetAxis("Mouse ScrollWheel") != 0 || isDragging)
            {
                float deltaY = Input.GetAxis("Mouse ScrollWheel");
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    deltaY = Input.GetTouch(0).deltaPosition.y / Screen.height;
                }

                MoveCamera(deltaY);
            }
        }
    }

    private bool IsEngagementAreaClicked()
    {
        // Convert screen portion to pixels
        Rect engagementArea = new Rect(Screen.width * engagementBounds.x, Screen.height * engagementBounds.y, Screen.width * engagementBounds.width, Screen.height * engagementBounds.height);

        // Check if the current mouse position is within the engagement area
        return engagementArea.Contains(Input.mousePosition);
    }

    void MoveCamera(float deltaY)
    {
        // Adjust the camera's position based on the input
        float newY = targetCamera.transform.position.y + deltaY * scrollSpeed;
        float newX = targetCamera.transform.position.x;
        float newZ = targetCamera.transform.position.z;
        targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, Mathf.Clamp(newY, newY, newY), targetCamera.transform.position.z);
    }

    private void EngageCameraControl()
    {
        isEngaged = true;
        Debug.Log("Camera Control Engaged!");
    }

    private void DisengageCameraControl()
    {
        isEngaged = false;
        Debug.Log("Camera Control Disengaged!");
    }
}