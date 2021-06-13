using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHelper : MonoBehaviour
{
    public Vector3 offset;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayA = new Ray(transform.position + offset, -transform.right);
        Ray rayB = new Ray(transform.position, -transform.right);
        RaycastHit hit;
        if (Physics.Raycast(rayB, out hit, distance))
        {
            if (hit.collider.gameObject.CompareTag("Floor"))
            {
                if (!Physics.Raycast(rayA, distance))
                {
                    Debug.Log("stepping up");
                    transform.position += new Vector3(0,0.5f);
                }
            }
            
        }
    }

}
