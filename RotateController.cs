//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//public class RotateController : MonoBehaviour
//{
//    private Vector3 currentPos { get => Input.mousePosition; set { } }
//    private Vector3 currentDelta { get => Input.mousePositionDelta; set { } }
//    private Vector3 previousPos;
//    private Vector3 previousDelta;
//    private Quaternion currentRotation { get => gameObject.transform.rotation; set { } }
//    private Direction direction;
//    private Quaternion nextRotation;
//    public float turnSpeed;

//    // Start is called before the first frame update
//    void Start()
//    {
//        previousDelta = currentDelta;
//        previousPos = currentPos;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    void Update()
//    {
//        Turn();

//        // Update previous position and delta for the next frame
//        previousPos = currentPos;
//        previousDelta = currentDelta;
//    }

//    void LateUpdate()
//    {
//        Turn();
//    }

//    private void FixedUpdate()
//    {
//        Turn();
//    }

//    void Turn()
//    {
//        float deltaX = currentDelta.x - previousDelta.x;
//        float deltaY = currentDelta.y - previousDelta.y;

//        float rotationX = deltaY * turnSpeed * Time.deltaTime;
//        float rotationY = -deltaX * turnSpeed * Time.deltaTime;
//        rotationX = rotationX > 45 ? 45 : rotationX;
//        rotationX = rotationX < -45 ? -45 : rotationX;
//        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);
//        nextRotation = Quaternion.Lerp(currentRotation, currentRotation * targetRotation, 0.1f);

//        // Apply the smoothed rotation to the object
//        transform.rotation = nextRotation;
//    }

//}

using System;
using UnityEditor;
using UnityEngine;
using Cursor = UnityEngine.Cursor;
using VQ = MyScripts.Eulers;

namespace MyScripts
{
	public static class Eulers
	{
		public static Vector3 QtoV3(Quaternion q)
		{
			return new Vector3(q.x, q.y, q.z);
		}
		public static Vector4 QtoV4(Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}
		public static Quaternion VToQ(Vector4 v)
		{
			return new Quaternion(v.x, v.y, v.z, v.w);
		}
		public static Quaternion VToQ(Vector2 v)
		{
			return new Quaternion(v.x, v.y, 0f, 0f);
		}
		public static Quaternion VToQ(Vector3 v)
		{
			return new Quaternion(v.x, v.y, v.z, 0f);
		}
		public static Vector3 TtoV3(Tuple<float,float,float> xyz)
		{
			return new Vector3(xyz.Item1, xyz.Item2, xyz.Item3);
		}
		public static Vector2 TtoV2(Tuple<float,float> xy)
		{
			return new Vector2(xy.Item1, xy.Item2);
		}
		public static Vector3 FtoV3(float? x, float? y, float? z)
		{
			if (x == null || y == null || z == null)
			{
				x = x == null ? x = 0f : (float)x;
				z = z == null ? z = 0f : (float)z;
				y = y == null ? y = 0f : (float)y;
			}
			return new Vector3((float)x, (float)y, (float)z);
		}
		public static Vector3 FtoV3(float? x, float? y)
		{
			if (x == null || y == null)
			{
				x = x == null ? x = 0f : (float)x;
				y = y == null ? y = 0f : (float)y;
			}
			return new Vector3((float)x, (float)y, 0f);
		}
		public static Vector3 FtoV3(float f, char a)
		{
			if (f == 0 || a.ToString().Length == 0)
			{
				return Vector3.zero;
			}
			Vector3 x;
			switch (a)
			{
				case 'x':
					x = new Vector3(f, 0f, 0f);
					break;
				case 'y':
					x = new Vector3(0f, f, 0f);
					break;
				case 'z':
					x = new Vector3(0f, 0f, f);
					break;
				default:
					x = new Vector3(0f, 0f, 0f);
					break;
			}
			return x;
		}



	}

	public class RotateController : MonoBehaviour
	{
	    [Range(0.5f,10f)]
	    public float turnSpeed = 1.1f;
	    public float rotationSmoothness = 0.1f;
	    public bool lockCursor = true;

	    private Vector2 currentMouseDelta { get => Input.mousePositionDelta; set { } }
	    private Vector2 smoothMouseDelta;
	    private Vector2 velocity;

	    void Start()
	    {
	        if (lockCursor)
	        {
	            Cursor.lockState = CursorLockMode.Confined;
	            Cursor.visible = false;
	        }
	    }

		private Vector2 lastMousePosition = new Vector2();
		private float lastRotationX = 0f;
		private float lastRotationY = 0f;
		private bool? updown { get => lastMousePosition.y > Input.mousePosition.y ? false : lastMousePosition.y == Input.mousePosition.y ? null : true; }
		private bool? sideside { get => lastMousePosition.x > Input.mousePosition.x ? false : lastMousePosition.x == Input.mousePosition.x ? null : true; }

		void ContinueMouseTracking()
		{
			if (Input.GetMouseButton(1))
			{
				// Calculate the rotation delta based on mouse movement
				Vector2 mouseDelta = (Vector2)Input.mousePosition - lastMousePosition;
				float rotationAmountX = mouseDelta.x * turnSpeed;
				if (rotationAmountX == 0)
				{
					if (sideside != null && sideside == false)
					{
						lastRotationX += -1f;
					}
					else if (sideside != null && sideside == true)
					{
						lastRotationX += 1f;
					}
					else
					{
						lastRotationX = turnSpeed * (turnSpeed / Math.Abs(Input.mousePosition.x));
					}
					rotationAmountX = lastRotationX;
				}
				// Apply rotation to the object
				transform.Rotate(Vector3.up, rotationAmountX, Space.World);
				// Update last mouse position
				lastRotationX = rotationAmountX;

				// Calculate the rotation delta based on mouse movement
				Vector2 mouseDeltaY = (Vector2)Input.mousePosition - lastMousePosition;
				float rotationAmountY = mouseDelta.y * turnSpeed;
				if (rotationAmountY == 0)
				{
					if (updown != null && updown == false)
					{
						lastRotationY += -1f;
					}
					else if(updown != null && updown == true)
					{
						lastRotationY += 1f;
					}
					else
					{
						lastRotationY = turnSpeed / Math.Abs(Input.mousePosition.y);
					}
					rotationAmountY = lastRotationY;
				}
				if (gameObject.transform.rotation.x <= -5f || transform.rotation.x >= 5f)
				{
					return;
					rotationAmountY = 0f;
					//transform.rotation.SetLookRotation(new (transform.rotation.x, transform.rotation.y,transform.rotation.z));
				}
				var r = transform.rotation;
				var lerpfrom = new Vector3(r.x, r.y, r.z);
				WhereTo(rotationAmountY, lerpfrom, 'x');
				// Update last mouse position
				lastMousePosition = Input.mousePosition;
				lastRotationY = rotationAmountY;
			}
		}

		void WhereTo(float lerpby, Vector3 lerpfrom, char axis)
		{
			// Apply rotation to the object
			var r = transform.rotation;
			Vector3 lerpto;
			if (lerpfrom != new Vector3())
			{
				//lerpto = new (CalculateXAngle(lerpfrom.x + lerpby, r.y, r.x, r.y),0f,0f);
				Quaternion x;
				switch (axis)
				{
					case 'x':
						x = new Quaternion(lerpby, 0f, 0f, transform.rotation.w);
						//x.eulerAngles = new Vector3(x.z, x.x, x.y);
						break;
					case 'y':
						x = new Quaternion(0f,lerpby, 0f, transform.rotation.w);
						//x.eulerAngles = new Vector3(x.z, x.x, x.y);
						break;
					case 'z':
						x = new Quaternion(0f, 0f, lerpby, transform.rotation.w);
						//x.eulerAngles = new Vector3(x.z, x.x, x.y);
						break;
					default:
						x = new Quaternion(0f, 0f, 0f, transform.rotation.w);
						//x.eulerAngles = new Vector3(x.z, x.x, x.y);
						break;
				}
				var ea = transform.rotation.eulerAngles;
				ea.Set(x.x,x.y,x.z);
				Debug.Log("ea.x-y-z" + ' ' + ea.x + ' ' + ea.y + ' ' +ea.z);
				//WhereTo(rotationAmountY, lerpto);
			}
		}


	    void Update()
	    {

			ContinueMouseTracking();
			return;

				var rmp = Display.RelativeMouseAt(Input.mousePosition);
			
				PlayModeWindow.GetRenderingResolution(out var w, out var h);
				if (Input.mousePosition.y >= h-1 || Input.mousePosition.y <= 1)
				{
					var tb = Input.mousePosition.y <= 0 ? 10 : Input.mousePosition.y >= h - 1 ? -10 : 0;
				Input.mousePosition.Set(Input.mousePosition.x, tb, 0);
			//	Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y + tb));
				}
				if (Input.mousePosition.x >= w-1 || Input.mousePosition.x <= 0)
				{
					var lr = Input.mousePosition.x <= 0 ? 10 : Input.mousePosition.x >= w - 1 ? -10 : 0;
				Input.mousePosition.Set(lr, Input.mousePosition.y, 0);
				//	Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x + lr, Input.mousePosition.y));
			}
			HandleMouseRotation();
	        //GetInputs();
	    }

	    //void HandleMouseRotation()
	    //{
	    //    if (Cursor.lockState == CursorLockMode.Locked)
	    //    {
	    //        // Calculate normalized position of the cursor on the x-axis
	    //        float normalizedX = (Input.mousePosition.x / Screen.width) * 2 - 1; // Map [0, Screen.width] to [-1, 1]

	    //        // Calculate the angle using arctangent
	    //        float angle = Mathf.Atan2(normalizedX, 1) * Mathf.Rad2Deg;

	    //        // Map the angle to the desired range of rotation
	    //        float targetRotationY = Mathf.Clamp(angle, -180f, 180f);

	    //        // Apply rotation
	    //        transform.rotation = Quaternion.Euler(0, targetRotationY, 0);
	    //    }
	    //}

	    private double lastx = 0.0;
	    private double lasty = 0.0;

	    void GetInputs() { 
	    double x = Input.mousePosition.x; // Replace with your x-coordinate
	    double y = Input.mousePosition.y;  // Replace with your y-coordinate
	    double cx = Screen.width / 2;  // Replace with your center x-coordinate
	    double cy = Screen.height / 2;  // Replace with your center y-coordinate
	    double xAngle = CalculateXAngle(x, y, cx, cy);
	        double yAngle = CalculateYAngle(x, y, cx, cy);
	        if (lastx != x || lasty != y)
	        {
	            Debug.Log(x.ToString() + " y:" + y.ToString() + " cx:" + cx.ToString() + " cy:" + cy.ToString() + " xangle:" + xAngle.ToString() + " yangle:" + yAngle.ToString());
	        }
	        lastx = x;
	        lasty = y;

	    }

	    static float CalculateXAngle(double x, double y, double cx, double cy)
	{
	    float angle = Mathf.Atan2((float)y, (float)x - (float)cx) * (180 / Mathf.PI);
	    return NormalizeAngle(angle);
	}

	static double CalculateYAngle(double x, double y, double cx, double cy)
	{
	    float angle = Mathf.Atan2((float)y - (float)cy, (float)x) * (180 / Mathf.PI);
	    return NormalizeAngle(angle);
	}

	static float NormalizeAngle(float angle)
	{
	    // Normalize the angle to be between -180 and 180 degrees
	    while (angle <= -180)
	    {
	        angle += 360;
	    }

	    while (angle > 180)
	    {
	        angle -= 360;
	    }

	    return angle;
	}
	    Vector2  MouseInput { get => (Input.mousePosition); set { } }


	    void HandleMouseRotation()
	    {
	        PlayModeWindow.GetRenderingResolution(out var w, out var h);
	        var circ = 2 * Math.PI * (w / 2);
	        circ = circ / 360;
	        var mainR = Camera.main.transform.rotation = Quaternion.identity;
	        var mainC = Camera.main;
	        mainC.transform.Rotate(0, 0, 0);

	        currentMouseDelta = Input.mousePosition;//new Vector2(Input.GetAxis("MouseX"), Input.GetAxis("MouseY"));
	        if (Cursor.lockState == CursorLockMode.Locked)
	        {
	            currentMouseDelta = Input.mousePosition;
	        }

	        smoothMouseDelta = Vector2.SmoothDamp(smoothMouseDelta, currentMouseDelta, ref velocity, rotationSmoothness);

	        float rotationX = -smoothMouseDelta.y * (turnSpeed / 10);
	        float rotationY = smoothMouseDelta.x * (turnSpeed / 10);
	        var eA = transform.eulerAngles;


	        Vector2 titties = smoothMouseDelta;
	        titties.Scale(new Vector2((float)(eA.x * (circ / 180)), ((float)(eA.y * (circ / 180)))));

	        eA.x = (eA.x + rotationX) * titties.x; 
	        eA.y = (eA.y + rotationY) * titties.y;
	        transform.eulerAngles = eA;
	        //Debug.Break();
	       transform.Rotate(new Vector3((rotationX * 2f), (rotationY * 2f), 0f));

	        //        transform.Rotate(Vector3.up, rotationY);
	    }
	}
}