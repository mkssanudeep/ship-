using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartSlot
{
    LeftLeg,
    LeftArm,
    RightLeg,
    RightArm,
    Body,
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
    private float yLimit;
    private float inputDisabledTimer;

    private Dictionary<PartSlot, GameObject> parts = new Dictionary<PartSlot, GameObject>();
    public List<Vector3> partPositions;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stepHelper = GetComponent<StepHelper>();
        stepHelper.enabled = false;
        yLimit = 1.5f;
    }

    public void Hit(int damage)
    {
        health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        inputDisabledTimer -= Time.deltaTime;

        colorShader.RadiusIndex = parts.Count;
        //take input and transate it to the camera
        if (inputDisabledTimer < 0)
        {
            inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rb.velocity = playerDirReference.transform.TransformDirection(new Vector3(inputVector.x * (speed - parts.Count), rb.velocity.y, inputVector.y * (speed - parts.Count)));

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

        if (parts.ContainsKey(PartSlot.LeftLeg) || parts.ContainsKey(PartSlot.RightLeg))
        {
            Stabilize();
            FaceMovementDirection();
        }
        if (parts.ContainsKey(PartSlot.LeftLeg) && parts.ContainsKey(PartSlot.RightLeg))
        {
            stepHelper.enabled = true;
            yLimit = transform.position.y + 0.2f;
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

    private void FaceMovementDirection()
    {
        if ((rb.velocity.x > 0.1 || rb.velocity.x < -0.1 || rb.velocity.z > 0.1 || rb.velocity.z < -0.1))
        {
            Vector3 fakefacing = Vector2.Perpendicular(new Vector2(-rb.velocity.x, -rb.velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(fakefacing.x, 0, fakefacing.y)), 2f);
        }
        
    }

    public void AddPart(GameObject g)
    {
        Debug.Log("adding part");
        if (g.CompareTag("Leg"))
        {
            if (!parts.ContainsKey(PartSlot.LeftLeg))
            {
                AddLeg(g, PartSlot.LeftLeg);

            }
            else if (!parts.ContainsKey(PartSlot.RightLeg))
            {
                AddLeg(g, PartSlot.RightLeg);
            }
            else
            {
                Debug.Log("No more leg slots");
            }
        }
    }

    public void AddLeg(GameObject g, PartSlot slot)
    {
        parts[slot] = g;
        g.transform.SetParent(transform);
        g.transform.rotation = new Quaternion(0, 0, 0, 0);
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
            if (g.CompareTag("Leg"))
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
        
    }
}
