using UnityEditor.Rendering;
using UnityEngine;

namespace MyScripts
{
    public class CrankController : MonoBehaviour
    {
        public Transform crankHandle;
        public Transform crankWheel;
        public Transform player;
        public float activationDistance = 5f;
        public KeyCode clockwiseKey = KeyCode.PageUp;
        public KeyCode counterClockwiseKey = KeyCode.PageDown;
        public float rotationSpeed = 50f;

        private bool isPlayerInRange = false;
        private bool isRotatingClockwise = false;
        private bool isRotatingCounterClockwise = false;

        void Update()
        {
            // Check if the player is within activation distance
            if (Vector3.Distance(transform.position, player.position) <= activationDistance)
            {
                isPlayerInRange = true;
            }
            else
            {
                isPlayerInRange = false;
            }

            // Check for clockwise rotation
            if (isPlayerInRange && Input.GetKey(clockwiseKey))
            {
                isRotatingClockwise = true;
                isRotatingCounterClockwise = false;
            }

            // Check for counter-clockwise rotation
            if (isPlayerInRange && Input.GetKey(counterClockwiseKey))
            {
                isRotatingCounterClockwise = true;
                isRotatingClockwise = false;
            }

            // Stop rotation when the key is released
            if (Input.GetKeyUp(clockwiseKey) || Input.GetKeyUp(counterClockwiseKey))
            {
                isRotatingClockwise = false;
                isRotatingCounterClockwise = false;
            }

            // Rotate the crank handle and wheel
            if (isRotatingClockwise)
            {
                RotateClockwise();
            }
            else if (isRotatingCounterClockwise)
            {
                RotateCounterClockwise();
            }
        }

        void RotateClockwise()
        {
            crankHandle.rotation.eulerAngles.Scale(new Vector3(0f, 0f, (float)(rotationSpeed * Time.deltaTime)));// crankWheel.TransformPoint(crankWheel.position)));
            crankWheel.rotation = new (0f, 0f, rotationSpeed * Time.deltaTime, crankWheel.TransformPoint(crankWheel.position).z);
        }

        void RotateCounterClockwise()
        {
            crankWheel.localPosition = new Vector3(0, 0, 0);
            crankWheel.position = crankWheel.TransformVector(crankWheel.localPosition);
            var cwea = crankWheel.rotation.eulerAngles;
            var cweaz = cwea.z;
            cwea.z = cweaz + -rotationSpeed * Time.deltaTime;

            crankWheel.eulerAngles = cwea;
 
            //crankHandle.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime, Space.Self);// (crankWheel.position / 2, Vector3.forward / 2, -rotationSpeed * Time.deltaTime);
//            crankWheel.Rotate(crankWheel.position, -rotationSpeed * Time.deltaTime);
        }

        //void RotateClockwise()
        //{
        //    Vector3 wheelPosLoc;
        //    Quaternion wheelRotLoc;
        //    crankWheel.GetLocalPositionAndRotation(out wheelPosLoc, out wheelRotLoc);
        //    crankWheel.localRotation.eulerAngles.Scale(wheelRotLoc.eulerAngles.normalized);
        //    crankHandle.Rotate(Vector3.Lerp(crankHandle.rotation.eulerAngles, -crankHandle.rotation.eulerAngles, rotationSpeed), rotationSpeed * Time.deltaTime);
        //   // crankWheel.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        //}

        //void RotateCounterClockwise()
        //{
        //    var handlePos = crankHandle.position;// (new Vector3(0,0,1), -rotationSpeed * Time.deltaTime);
        //    var wheelPos = crankWheel.position;
        //    var ea = crankHandle.eulerAngles;
        //    var eaw = crankWheel.eulerAngles;
        //    ea.z = crankHandle.rotation.eulerAngles.normalized.z * (rotationSpeed * Time.deltaTime);//Vector3.forward, -rotationSpeed * Time.deltaTime);
        //    eaw.z = crankWheel.rotation.eulerAngles.normalized.z * (rotationSpeed * Time.deltaTime);//Vector3.forward, -rotationSpeed * Time.deltaTime);
        //    crankHandle.position = handlePos;
        //    //crankWheel.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        //    //crankWheel.position = wheelPos;
        //    //var comass = crankWheel.gameObject.GetComponent<Rigidbody>().worldCenterOfMass;
        //    //crankWheel.RotateAround(comass, crankWheel.rotation.z + rotationSpeed * Time.deltaTime);
        //}
    }
}
