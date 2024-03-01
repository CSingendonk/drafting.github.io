using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EndTriggerListener : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject playerCapsule;
    public GameObject tileObject;
    private Vector3 triggerPosition;
    private Quaternion triggerRotation;
    private Vector3 triggerScale;
    private Vector3 playerPosition;
    private Quaternion playerRotation;
    private Vector3 playerScale;
    private Vector3 playerCapPosition;
    private Quaternion playerCapRotation;
    private Vector3 playerCapScale;

    private List<Vector3> vector3s = new List<Vector3>();

    private void CheckPositions(Collider other)
    {
        playerPosition = playerCapsule.transform.position;
        var otherPos = other.transform.position;
        if (otherPos != playerPosition)
        {
            playerPosition = otherPos;
        }
        triggerPosition = gameObject.transform.position;
        var triggerBounds = gameObject.GetComponent<BoxCollider>().bounds;
        if (triggerBounds != null)
        {
            if (triggerBounds.Contains(playerPosition))
            {
                var capPosition = playerCapsule.transform.position;
                float ppy = (playerPosition.y + 10f);
                playerPosition = new(0f, ppy, 0f);
                playerCapsule.transform.position = playerPosition;
                playerCapsule.transform.Translate(0, ppy, 0);
            };
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // CheckPositions(playerCapsule.GetComponent<CapsuleCollider>());
        playerCapsule.transform.localPosition = new Vector3(0, 2, 0);
        if (tileObject.transform.position != playerPosition)
        {
            playerPosition = playerCapsule.transform.position;
            var x = triggerPosition.x + 0.5;
            var xx = triggerPosition.x - 0.5;
            var z = tileObject.transform.position.z + 0.5;
            var zz = tileObject.transform.position.z - 0.5;
            var px = playerCapPosition.x;
            var pz = playerCapPosition.z;
            if (px < x && px > xx)
            {
                if (pz < z && pz > zz)
                {
                    Debug.Log("playerCapsule x,z should be within tileObject x,z bounds.");
                    Debug.Log(("x=", x.ToString(), " ", xx.ToString(), "z=", z.ToString(), " ", zz.ToString()).ToString());
                    backToStart();
                }
            }

            Debug.Log(px.ToString() + " " + pz.ToString());
        }
    }

    void UpdateGlobalVars()
    {
        playerPosition = playerObject.transform.position;
        playerRotation = playerObject.transform.rotation;
        playerScale = playerObject.transform.localScale;
        playerCapPosition = playerCapsule.transform.position;
        playerCapRotation = playerCapsule.transform.rotation;
        playerCapScale = playerCapsule.transform.localScale;
        triggerPosition = gameObject.transform.position;
        triggerRotation = gameObject.transform.rotation;
        triggerScale = gameObject.transform.localScale;
    }


    void backToStart()
    {
        //           .lerp.position.y, lerp.position.x) ;
        UpdateGlobalVars();
        playerObject.transform.Translate(0 - playerPosition.x, 0, 0 - playerCapPosition.z, Space.World);
        UpdateGlobalVars();
        playerCapsule.transform.Translate(playerCapPosition, Space.World);
        UpdateGlobalVars();
        playerObject.transform.SetPositionAndRotation(new Vector3(0, 2, 0), new Quaternion(0, 0, 0, 0));
        UpdateGlobalVars();
        playerCapsule.transform.SetLocalPositionAndRotation(new Vector3(0, 2, 0), playerRotation);
    
    }

}
