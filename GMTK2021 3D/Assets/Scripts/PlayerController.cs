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
        colorShader.RadiusIndex = parts.Count;
        //take input and transate it to the camera
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = playerDirReference.transform.TransformDirection(new Vector3(inputVector.x * speed, rb.velocity.y, inputVector.y * speed));

        
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
        }
        else
        {
            stepHelper.enabled = false;
        }
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
            Debug.Log("facing forward");
            Vector3 fakefacing = Vector2.Perpendicular(new Vector2(-rb.velocity.x, -rb.velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(fakefacing.x, 0, fakefacing.y)), 5f);
        }
        
    }

    public void AddPart(GameObject g)
    {
        Debug.Log("adding part");
        if (g.CompareTag("Leg"))
        {
            if (!parts.ContainsKey(PartSlot.LeftLeg))
            {
                Debug.Log("setting left");
                parts[PartSlot.LeftLeg] = g;
                g.transform.SetParent(transform);
                g.transform.localPosition = partPositions[(int)PartSlot.LeftLeg];
                g.transform.rotation = new Quaternion(0, 0, 0, 0);
                FixedJoint j = g.AddComponent<FixedJoint>();
                j.anchor = partPositions[(int)PartSlot.LeftLeg];
                j.connectedBody = rb;

            }
            else if (!parts.ContainsKey(PartSlot.RightLeg))
            {
                Debug.Log("setting right");
                parts[PartSlot.RightLeg] = g;
                g.transform.SetParent(transform);
                g.transform.rotation = new Quaternion(0, 0, 0, 0);
                g.transform.localPosition = partPositions[(int)PartSlot.RightLeg];
                FixedJoint j = g.AddComponent<FixedJoint>();
                j.anchor = partPositions[(int)PartSlot.RightLeg];
                j.connectedBody = rb;


            }
            else
            {
                Debug.Log("No more leg slots");
            }
        }
    }

    public void EjectPart(GameObject g)
    {
        Debug.Log("yeet");
        if (g != null)
        {
            if (g.CompareTag("Leg"))
            {
                g.transform.SetParent(null);
                Destroy(g.GetComponent<FixedJoint>());
                g.GetComponent<Part>().interaction.gameObject.SetActive(true);
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 force = Vector3.Normalize(new Vector3(transform.position.x - mouseWorldPos.x, transform.position.y, transform.position.z - mouseWorldPos.z));
                g.GetComponent<Rigidbody>().AddForce(force * 4);
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
