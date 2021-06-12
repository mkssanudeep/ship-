using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public GameObject target;
    //public Vector3 offset;

    public GameObject blocking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.transform.position + offset;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == 7)
            {
                blocking = hit.collider.gameObject;
                blocking.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                if (blocking != null)
                {
                    blocking.GetComponent<MeshRenderer>().enabled = true;
                    blocking = null;
                }
               
            }
        }
    }
}
