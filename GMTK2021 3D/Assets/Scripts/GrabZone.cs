using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabZone : MonoBehaviour
{
    public List<GameObject> movableObjects;
    public List<GameObject> keys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Movable"))
        {
            movableObjects.Add(other.gameObject);
        }
        else if (other.CompareTag("Key"))
        {
            keys.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Movable"))
        {
            movableObjects.Remove(other.gameObject);
            other.gameObject.GetComponent<Rigidbody>().mass = 100000;
            Destroy(other.gameObject.GetComponent<FixedJoint>());
        }
        else if (other.CompareTag("Key"))
        {
            keys.Remove(other.gameObject);
        }
    }
}
