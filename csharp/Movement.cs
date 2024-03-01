using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


//Author: Christopher Singendonk
//Date: January 17 2024
public class Movement : MonoBehaviour
{
    [Range(0.01f, 1.00f)]
    public float strafeSpeed;
    private float x;
    [Range(0.01f, 1.00f)]
    public float walkSpeed;
    private float z;
    private float diagonalSpeed { get => (this.strafeSpeed + this.walkSpeed) / 2; set { } }
    private float xz;
    private float jumpHeight { get => wallObject.transform.localScale.y / 10; set { } }
    private float y;
    private float runFactor = 2.321f;
    private float runSpeed { get => this.walkSpeed * this.runFactor; set { } }
    private float sneakFactor = 0.123f;
    private float sneakSpeed { get => this.walkSpeed * this.sneakFactor; set { } }
    private float backupSpeed { get => this.runSpeed * this.sneakFactor; }
    private bool jvalue { get; set; }
    private Rigidbody rigidBody { get; set; }
#nullable enable
    public GameObject? groundObject;
#nullable disable
    public GameObject mazeObject;
    public GameObject wallObject;
    public Collider[] Colliders { get => this.GetColliders(); set { } }
    private float groundY { get => this.groundObject.transform.position.y; set { } }
    private float groundTopY { get => this.groundY + (this.groundObject.transform.localScale.y / 2); set { } }
    private float objectBottomY { get => gameObject.transform.position.y - (gameObject.transform.localScale.y / 2); set { } }
    private Dictionary<string, KeyCode> ArrowKeyCodes { get; set; } = new Dictionary<string, KeyCode>();
    private List<KeyCode> keysDownCodes { get; set; }
    private Quaternion Tform { get => gameObject.transform.rotation; set { } }

    // Straight from the...
    void Start()
    {
        ArrowKeyCodes.Add("N", KeyCode.UpArrow);
        ArrowKeyCodes.Add("E", KeyCode.RightArrow);
        ArrowKeyCodes.Add("S", KeyCode.DownArrow);
        ArrowKeyCodes.Add("W", KeyCode.LeftArrow);
        ArrowKeyCodes.Add("F", KeyCode.W);
        ArrowKeyCodes.Add("R", KeyCode.D);
        ArrowKeyCodes.Add("B", KeyCode.S);
        ArrowKeyCodes.Add("L", KeyCode.A);
        x = (0.1f * strafeSpeed) / 2;
        strafeSpeed = x;
        z = (0.1f * walkSpeed) / 2;
        walkSpeed = z;
        y = jumpHeight;
        gameObject.AddComponent(typeof(Rigidbody));
        rigidBody = gameObject.GetComponent<Rigidbody>();
        keysDownCodes = KeysDown();
        MoveBitch.MoveBitchgtfo(this);
        GetColliders();
    }

    Collider[] GetColliders()
    {
        var i = 0;
        var cz = mazeObject.transform.GetComponentsInChildren<Collider>();
        var colliderGroup = new Collider[cz.Length];
        foreach (Collider w in cz)
        {
            colliderGroup[i] = w;
            i = i < cz.Length-1 ? i++ : 0;
        }
        return colliderGroup;
    }

    // This just in...
    void Update()
    {
        if (Input.anyKey)
        {
            if (keysDownCodes != null && keysDownCodes.Count > 0)
            {
                keysDownCodes.Clear();
            }
            KeysDown();
            if (keysDownCodes.Count >= 1)
            {
                MoveTo();
                //MoveDiagonally(keysDownCodes[0], keysDownCodes[1]);
                return;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            //Input.GetKey(WhichKey());
        }
    }

    // There
    bool MoveTo()
    {
        var keyCodeA = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ? (Input.GetKey(KeyCode.A) ? KeyCode.A : KeyCode.D) : KeyCode.None;
        var keyCodeB = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ? (Input.GetKey(KeyCode.W) ? KeyCode.W : KeyCode.S) : KeyCode.None;
        if (keyCodeA == KeyCode.None)
        {
            keyCodeA = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ? (Input.GetKey(KeyCode.LeftArrow) ? KeyCode.LeftArrow : KeyCode.RightArrow) : KeyCode.None;
        }
        if (keyCodeB == KeyCode.None)
        {
            keyCodeB = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ? (Input.GetKey(KeyCode.UpArrow) ? KeyCode.UpArrow : KeyCode.DownArrow) : KeyCode.None;
        }
        if (keyCodeA != KeyCode.None && keyCodeB != KeyCode.None)
        {
            MoveXZ(new Vector3(GetSideStep(keyCodeA), GetStride(keyCodeB)));
            return true;
        }
        else if (keyCodeA != KeyCode.None && keyCodeB == KeyCode.None)
        {
            MoveXZ(new Vector3(GetSideStep(keyCodeA), 0f, 0f));
            return true;
        }
        else if (keyCodeA == KeyCode.None && keyCodeB != KeyCode.None)
        {
            MoveXZ(new Vector3(0f, 0f, GetStride(keyCodeB)));
            return true;
        }
        if (KeysDown().Count <= 1)
        {
            Invoke("MoveTo", 0.1f);
        }
        return false;
    }

    // The opposite of StationaryXZ
    void MoveXZ(Vector3 v)
    {
        //var why = groundObject.transform.position.y;
        //gameObject.transform.Translate(v, Space.Self);

        // Store the initial position
        Vector3 initialPosition = gameObject.transform.position;
        if (CheckWalls(initialPosition))
        {
            return;
        }
        // Transform the local movement vector to global coordinates
        Vector3 globalMovement = gameObject.transform.TransformDirection(v);
        // Apply the global movement to the position
        gameObject.transform.position += globalMovement;

        // Get the new global position
        Vector3 newPosition = gameObject.transform.position;

        // Keep the y position constant globally
        newPosition.y = initialPosition.y;


        // Update the position
        gameObject.transform.position = newPosition;
    }

    // That way
    Vector3 GetDirection(float a, float b)
    {
        Vector3 v = new Vector3(x: a, 0f, z: b);
        return v;
    }


    // Which one?
    // Works but not used in the current iteration.
    public KeyCode WhichKey()
    {

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            var k = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) ? KeyCode.LeftArrow : (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ? KeyCode.RightArrow : KeyCode.None);
            if (Input.GetKey(k) && k != KeyCode.None)
            {
                Strafe(k);
                return k;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            var k = Input.GetKey(KeyCode.UpArrow) ? KeyCode.UpArrow : KeyCode.DownArrow;
            if (Input.GetKey(k))
            {
                Walk(k);
                return k;
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
            return KeyCode.Space;
        }
        return KeyCode.None;
    }

    // Uno? Dos? Si.
    bool ModKeyDown(KeyCode firstKey, KeyCode secondaryKey)
    {
        return (Input.GetKey(firstKey) && Input.GetKey(secondaryKey));
    }

    // Jump around! Jump around! Jump up, jump up and get down! - House of Pain
    void Jump()
    {
        jvalue = Input.GetKey(KeyCode.Space);
        var high = gameObject.transform.position.y;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            groundY = groundObject.transform.position.y;
            groundY = groundY < 0 ? 0 : (groundY > groundTopY ? 0 : groundTopY);
        }
        if (high >= ((wallObject.transform.localScale.y * 0.75) + groundY))
        {
            rigidBody.useGravity = true;
            rigidBody.isKinematic = rigidBody.useGravity ? false : true;
            return;
        }
        if (jvalue)
        {
            if (jumpHeight != y)
            {
                jumpHeight = y;
            }
            gameObject.transform.Translate(new Vector3(0, jumpHeight, 0));
        }
    }

    // Step distance
    float GetSideStep(KeyCode firstKey)
    {
        strafeSpeed = walkSpeed * 0.75f;
        if (ModKeyDown(firstKey, KeyCode.LeftShift) || ModKeyDown(firstKey, KeyCode.RightShift))
        {
            strafeSpeed = runSpeed * 0.75f;
        }
        float d = firstKey == KeyCode.LeftArrow || firstKey == KeyCode.A ? -1f : 1f;
        return d * strafeSpeed;
    }

    // Step distance
    float GetStride(KeyCode firstKey)
    {
        if (walkSpeed != z)
        {
            walkSpeed = z;
        }
        if (ModKeyDown(firstKey, KeyCode.LeftShift) || ModKeyDown(firstKey, KeyCode.RightShift))
        {
            walkSpeed = runSpeed;
        }
        var d = firstKey == KeyCode.UpArrow || firstKey == KeyCode.W ? 1 : -1;
        if (d > 0)
        {
            return d * walkSpeed;
        }
        if (d < 0)
        {
            return d * backupSpeed + (0 - walkSpeed);
        }
        return d;
    }


    // Works but not used in the current iteration.
    KeyCode Walk(KeyCode keyDown)
    {

        if (walkSpeed != z)
        {
            walkSpeed = z;
        }
        if (ModKeyDown(keyDown, KeyCode.LeftShift) || ModKeyDown(keyDown, KeyCode.RightShift))
        {
            walkSpeed = runSpeed;
        }
        if (ModKeyDown(keyDown, KeyCode.LeftArrow) || ModKeyDown(keyDown, KeyCode.RightArrow))
        {
            diagonalSpeed = (strafeSpeed + walkSpeed) * sneakFactor;
            walkSpeed = diagonalSpeed;
            strafeSpeed = walkSpeed;
        }

        int lr;
        float direction = 0;
        Vector3 travel = new Vector3();

        switch (keyDown)
        {
            case KeyCode.UpArrow:
                lr = 1;
                direction = (float)lr * walkSpeed;
                break;
            case KeyCode.DownArrow:
                lr = -1;
                direction = (float)lr * this.backupSpeed;
                break;
            default:
                lr = 0;
                break;
        }


        travel = new Vector3(0, 0, direction * walkSpeed);
        gameObject.transform.Translate(travel, Space.Self);
        if (KeysDown().Count <= 1)
        {
            Invoke("WhichKey", 0.1f);
        }
        else
        {
            return keyDown;
        }
        return KeyCode.None;
    }

    // Checks for specified keyboard switches states. Counts them. Returns list of pressed switches from specified.
    List<KeyCode> KeysDown()
    {
        int i = 0;
        List<KeyCode> keys = keysDownCodes == null ? (keysDownCodes = new List<KeyCode>()) : keysDownCodes;
        if (keys.Count > 0)
        {
            keys.Clear();
            keysDownCodes.Clear();
        }
        foreach (KeyCode k in ArrowKeyCodes.Values)
        {
            if (Input.GetKey(k))
            {
                i++;
                keys.Add(k);
            }
        }
        return keys;
    }

    // Works but not used in the current iteration.
    public KeyCode Strafe(KeyCode keyDown)
    {
        if (strafeSpeed != x)
        {
            strafeSpeed = x;
        }
        if (ModKeyDown(keyDown, KeyCode.LeftShift) || ModKeyDown(keyDown, KeyCode.RightShift))
        {
            strafeSpeed = x * runFactor;
        }
        if (ModKeyDown(keyDown, KeyCode.UpArrow) || ModKeyDown(keyDown, KeyCode.DownArrow))
        {
            strafeSpeed = diagonalSpeed;
            KeyCode k2 = Input.GetKey(KeyCode.UpArrow) ? KeyCode.UpArrow : KeyCode.DownArrow;
            if (k2 == KeyCode.DownArrow)
            {
                strafeSpeed = diagonalSpeed * sneakFactor;
            }
            walkSpeed = strafeSpeed;
            Walk(k2);
        }
        int lr;
        float direction;
        Vector3 travel = new Vector3();

        switch (keyDown)
        {
            case KeyCode.LeftArrow:
                lr = -1;
                break;
            case KeyCode.RightArrow:
                lr = 1;
                break;
            default:
                lr = 0;
                break;
        }

        direction = (float)lr * strafeSpeed;
        travel = new Vector3(direction * strafeSpeed, 0, 0);
        gameObject.transform.Translate(travel, Space.Self);
        if (KeysDown().Count <= 1)
        {
            Invoke("WhichKey", 0.1f);
        }
        else
        {
            return keyDown;
        }
        return KeyCode.None;
    }

    // Works but not used in the current iteration.
    void MoveDiagonally(KeyCode key1, KeyCode key2)
    {
        var keyCodes = keysDownCodes;
        if (keyCodes.Contains(key1) && keyCodes.Contains(key2))
        {
            if (key1 == KeyCode.UpArrow || key1 == KeyCode.DownArrow)
            {
                Walk(key1);
                if (key2 == KeyCode.LeftArrow || key2 == KeyCode.RightArrow)
                {
                    Strafe(key2);
                }
            }
            else if (key1 == KeyCode.LeftArrow || key1 == KeyCode.RightArrow)
            {
                Strafe(key1);
                if (key2 == KeyCode.UpArrow || key2 == KeyCode.DownArrow)
                {
                    Walk(key2);
                }
            }
            if (keyCodes.Count == 1)
            {
                Invoke("WhichKey", 0.1f);
            }
        }
    }

    bool CheckWalls(Vector3 newPos)
    {
        if (Colliders.Length == 0 || Colliders == null)
        {
            GetColliders();
        }
        foreach (var w in Colliders)
        {
            if (w != null)
            {
                var e = w.GetComponent<Collider>();
                if (e.gameObject.tag != "ground")
                {
                    if (e.bounds.Contains(gameObject.GetComponent<Collider>().ClosestPointOnBounds(e.bounds.center)))
                    {
                        return true;
                    }
                }
                else
                {
                    Detour(w.bounds.center, gameObject.transform.position, gameObject.transform.rotation);
                    break;
                }
            }
        }
        return false;
    }

    void OnCollisionExit(Collision collision)
    {
        Roadblock(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        Roadblock(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        Roadblock(collision);
    }

    void Roadblock(Collision collision)
    {
        if (collision.collider != null)
        {
            Vector3 pos; // = Vector3.zero;
            Quaternion rot; // = new Quaternion();
            collision.collider.transform.GetPositionAndRotation(out pos, out rot);
            if (this.gameObject.GetComponent<Collider>().bounds.ClosestPoint(gameObject.transform.position).z <= pos.z)
            {
                Debug.Log("pos = " + pos + " collision = " + collision);
                if (RoadClosed(pos, collision.collider, rot, collision))
                {
                    var d = Detour(pos, gameObject.transform.position, gameObject.transform.rotation);
                    if (d != Vector3.zero && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
                    {
                        gameObject.transform.Translate(d);
                        this.MoveXZ(d);
                    }
                }
            }
        }
    }
    private bool RoadClosed(Vector3 pos, Collider collision, Quaternion r, Collision collider)
    {
        var perp = gameObject.GetComponent<Collider>();
        var perpWhere = perp.bounds;
        var vicWhere = collision.bounds;
        var perpLKP = perpWhere.center.normalized;
        var where = vicWhere.Contains(perpLKP);
        return where;
    }
    private Vector3 TrafficJam(Vector3 vicWhere, Vector3 perpWhere, Quaternion headed)
    {
        var latitude = vicWhere.x > perpWhere.x ? 1 : (vicWhere.x != perpWhere.x ? -1 : 0);
        var longitude = vicWhere.z > perpWhere.z ? 1 : (vicWhere.z != perpWhere.z ? -1 : 0);
        var facing = new Quaternion();
        gameObject.transform.GetPositionAndRotation(out vicWhere, out facing);
        if (facing.eulerAngles.x == headed.eulerAngles.x && facing.eulerAngles.z == headed.eulerAngles.z)
        {
            return Vector3.zero;
        }

        return Vector3.forward;
    }

    private Vector3 Detour(Vector3 vicWhere, Vector3 perpWhere, Quaternion headed)
    {
        var latitude = vicWhere.x > perpWhere.x ? 1 : (vicWhere.x != perpWhere.x ? -1 : 0);
        var longitude = vicWhere.z > perpWhere.z ? 1 : (vicWhere.z != perpWhere.z ? -1 : 0);

        var facing = gameObject.transform.rotation;

        // Calculate the local forward vector in the direction of the headed quaternion
        Vector3 localForward = facing * Vector3.forward;

        // Perform a raycast to check for collisions
        RaycastHit hit;
        if (Physics.Raycast(vicWhere, localForward, out hit, 0.01f))
        {
            // Check if the hit point is between perpWhere and vicWhere
            if (Vector3.Dot(perpWhere - vicWhere, hit.point - vicWhere) > 0 &&
                Vector3.Dot(vicWhere - perpWhere, hit.point - perpWhere) > 0)
            {
                // There would be a collision, return Vector3.zero
                return Vector3.zero;
            }
        }

        // No collision detected, return the local forward vector
        return localForward;
    }

}

// Move bitch, get out the way! Get out the way bitch! Get out the way! - Ludacris
public static class MoveBitch
    {
    public static void MoveBitchgtfo(Movement m)
    { getshit(m); }

    public static void getshit(Movement m)
    {

        Movement movement = m;
        var a = typeof(PropertyInfo).GetMembers();
        var b = typeof(Movement).GetFields();
        var c = typeof(Movement).GetConstructors();
        var d = typeof(MyScripts.ExitButtonHandler).GetMethods();
        var e = movement.gameObject;
        var f = movement.GetInstanceID();
        }
    static void GETOUTTHEWAY(object getoutthewaybitch)
    {
        GETOUTTHEWAY(null);
    }
    }


