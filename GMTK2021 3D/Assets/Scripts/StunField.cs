using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunField : MonoBehaviour
{
    public List<StalwartController> objectsInField;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stalwart"))
        {
            objectsInField.Add(other.gameObject.GetComponent<StalwartController>());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInField.Remove(other.gameObject.GetComponent<StalwartController>());
    }
}
