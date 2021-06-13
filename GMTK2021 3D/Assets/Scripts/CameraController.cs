using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public GameObject target;
    //public Vector3 offset;

    public List<GameObject> blocking;
    public GameObject PlayerObject;
    GameObject PrevBlock;
    Ray ray;
    Vector3 CameraDirection;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerObject == null)
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.transform.position + offset;

        //Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        CameraDirection = (this.transform.position - PlayerObject.transform.position).normalized;
        RaycastHit hit;
        Debug.DrawRay(PlayerObject.transform.position, CameraDirection);
        if (Physics.Raycast(PlayerObject.transform.position, CameraDirection, out hit))
        {
            if (hit.collider.gameObject.layer == 7)
            {
                if (PrevBlock != hit.collider.gameObject)
                {
                    if (PrevBlock != null)
                    {
                        PrevBlock.GetComponent<MeshRenderer>().enabled = true;
                        blocking.Remove(PrevBlock);
                    }
                    PrevBlock = hit.collider.gameObject;
                    blocking.Add(hit.collider.gameObject);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }                
            }
            else
            {
                if (PrevBlock != null)
                {
                    PrevBlock.GetComponent<MeshRenderer>().enabled = true;
                    blocking.Remove(PrevBlock);
                    PrevBlock = null;
                }

            }
        }
        else
        {
            if (PrevBlock != null)
            {
                PrevBlock.GetComponent<MeshRenderer>().enabled = true;
                blocking.Remove(PrevBlock);
                PrevBlock = null;
            }

        }
    }
}
