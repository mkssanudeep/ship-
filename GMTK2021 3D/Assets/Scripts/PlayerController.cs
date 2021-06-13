using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartSlot
{
    LeftLeg,
    LeftArm,
    RightLeg,
    RightArm,
    Bottom,
    None
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public Vector2 inputVector;
    public GameObject playerDirReference;
    public GameObject playerContainer;
    public float speed;
    public int health;
    public Stun stun;
    private StepHelper stepHelper;
    public ShaderPointTracker colorShader;
    private float inputDisabledTimer;
    private Vector2 fakefacing;

    private GameObject movableObject;

    private Dictionary<PartSlot, GameObject> parts = new Dictionary<PartSlot, GameObject>();
    public List<Vector3> partPositions;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stepHelper = GetComponent<StepHelper>();
        stepHelper.enabled = false;
    }

    public void Hit(int damage)
    {
        health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        fakefacing = Vector2.Perpendicular(new Vector2(-rb.velocity.x, -rb.velocity.z));
        inputDisabledTimer -= Time.deltaTime;

        colorShader.RadiusIndex = parts.Count;
        //take input and transate it to the camera
        if (inputDisabledTimer < 0)
        {
            inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rb.velocity = playerDirReference.transform.TransformDirection(new Vector3(inputVector.x * (speed - parts.Count), 0, inputVector.y * (speed - parts.Count)));

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject part = null;
            foreach (KeyValuePair<PartSlot, GameObject> kv in parts)
            {
                if (kv.Value != null)
                {

                    part = kv.Value;
                }

            }
            EjectPart(part);
        }

        //if either slot has a leg, the character will stay upright and face the direction of movement
        if (parts.ContainsKey(PartSlot.LeftLeg) || parts.ContainsKey(PartSlot.RightLeg))
        {
            Stabilize();
            FaceMovementDirection();
        }

        if (parts.ContainsKey(PartSlot.RightArm) || parts.ContainsKey(PartSlot.LeftArm))
        {
            Pull();
        }

        //if both slots have a leg, the character will hop up steps with ease
        if (parts.ContainsKey(PartSlot.LeftLeg) && parts.ContainsKey(PartSlot.RightLeg))
        {
            stepHelper.enabled = true;
        }
        else
        {
            stepHelper.enabled = false;
        }
    }

    public void DisableInput(float delay)
    {
        inputDisabledTimer = delay;
    }

    private void Stabilize()
    {
        var angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > 0.001)
        {
            var axis = Vector3.Cross(transform.up, Vector3.up);
            rb.AddTorque(axis * angle * 1f);
        }
    }

    private void Pull()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (movableObject != null)
            {
                movableObject.GetComponent<Rigidbody>().mass = 10;
                FixedJoint j = movableObject.AddComponent<FixedJoint>();
                j.connectedBody = rb;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (movableObject != null)
            {
                movableObject.GetComponent<Rigidbody>().mass = 1000000;
                Destroy(movableObject.GetComponent<FixedJoint>());
            }
        }
    }

    private void FaceMovementDirection()
    {
        if ((rb.velocity.x > 0.1 || rb.velocity.x < -0.1 || rb.velocity.z > 0.1 || rb.velocity.z < -0.1))
        {
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(fakefacing.x, 0, fakefacing.y)), 2f);
        }
        
    }

    public void AddPart(GameObject g)
    {
        Debug.Log("adding part");
        if (g.CompareTag("Leg"))
        {
            if (SlotFree(0))
            {
                AddPart(g, PartSlot.LeftLeg);

            }
            else if (SlotFree(1))
            {
                AddPart(g, PartSlot.RightLeg);
            }
            else
            {
                Debug.Log("No slot available");
            }
        }
        else if (g.CompareTag("Arm"))
        {
            if (SlotFree(0))
            {
                AddPart(g, PartSlot.LeftArm);
            }
            else if (SlotFree(1))
            {
                AddPart(g, PartSlot.RightArm);
            }
            else
            {
                Debug.Log("No slot available");
            }
        }
    }

    //the index is as follows 0=left, 1=right, 2=bottom
    private bool SlotFree(int index)
    {
        switch (index)
        {
            case 0:
                if (!parts.ContainsKey(PartSlot.LeftLeg) && !parts.ContainsKey(PartSlot.LeftArm))
                {
                    return true;
                }
                break;
            case 1:
                if (!parts.ContainsKey(PartSlot.RightLeg) && !parts.ContainsKey(PartSlot.RightArm))
                {
                    return true;
                }
                break;
            case 2:
                if (!parts.ContainsKey(PartSlot.Bottom))
                {
                    return true;
                }
                break;
        }
        return false;
    }

    public void AddPart(GameObject g, PartSlot slot)
    {
        parts[slot] = g;
        g.transform.SetParent(transform);
        if (slot == PartSlot.LeftArm || slot == PartSlot.RightArm)
        {
            g.transform.rotation = Quaternion.LookRotation(new Vector3(fakefacing.x, 0, fakefacing.y));
        }
        else
        {
            g.transform.rotation = new Quaternion(0, 0, 0, 0);

        }
        
        g.transform.localPosition = partPositions[(int)slot];
        FixedJoint j = g.AddComponent<FixedJoint>();
        j.anchor = partPositions[(int)slot];
        j.connectedBody = rb;
    }

    public void EjectPart(GameObject g)
    {
        Debug.Log("yeet");
        if (g != null)
        {
            Vector3 position = new Vector3();
            Plane plane = new Plane(Vector3.up, 0);
            plane.SetNormalAndPosition(Vector3.up, new Vector3(0, transform.position.y, 0));

            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                position = ray.GetPoint(distance);
            }
            g.transform.SetParent(null);
            Destroy(g.GetComponent<FixedJoint>());
            Part p = g.GetComponent<Part>();
            p.interaction.gameObject.SetActive(true);
            p.charged = true;
            Vector3 force = Vector3.Normalize(new Vector3(position.x - transform.position.x, transform.position.y, position.z - transform.position.z));
            g.GetComponent<Rigidbody>().AddForce(force * 10, ForceMode.Impulse);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            force = new Vector3(force.x, -0.2f, force.z);
            rb.AddForce(-force * 5, ForceMode.Impulse);
            DisableInput(0.5f);

            //Remove the key,value that held the object 
            PartSlot key = PartSlot.None;
            foreach (KeyValuePair<PartSlot, GameObject> kv in parts)
            {
                if (kv.Value == g)
                {
                    key = kv.Key;
                }
            }
            parts.Remove(key);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Movable"))
        {
            movableObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Movable"))
        {
            if (movableObject == collision.gameObject)
            {
                Destroy(collision.gameObject.GetComponent<FixedJoint>());
                movableObject = null;
            }
        }
    }
}
