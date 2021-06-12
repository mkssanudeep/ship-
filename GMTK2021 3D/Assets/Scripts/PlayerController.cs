using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartSlot
{
    LeftLeg,
    LeftArm,
    RightLeg,
    RightArm,
    Body
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public Vector2 inputVector;
    public GameObject playerDirReference;
    public float speed;
    public int health;
    public Stun stun;

    private Dictionary<PartSlot, GameObject> parts = new Dictionary<PartSlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //set up the slots in the parts dictionary
        for (int i = 0; i < 5; i++)
        {
            parts[(PartSlot)i] = null;
        }
    }

    public void Hit(int damage)
    {
        health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        //take input and transate it to the camera
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = playerDirReference.transform.TransformDirection(new Vector3(inputVector.x * speed, rb.velocity.y, inputVector.y * speed));

        
    }
}
