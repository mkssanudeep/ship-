using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public InteractionField interaction;
    public bool charged;
    public Stun stun;
    public ParticleSystem zap;

    public List<GameObject> groundTouching;

    HighlightScript highlight;
    // Start is called before the first frame update
    void Start()
    {
        highlight = GetComponent<HighlightScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interaction.isActiveAndEnabled && interaction.player != null)
        {
            highlight.TurnOnHighlights();
            if (Input.GetKeyDown(KeyCode.E))
            {
                //DisablePhysics();
                interaction.player.GetComponent<PlayerController>().AddPart(gameObject);
                
            }
        }
        else
        {
            highlight.TurnOffHighlights();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (charged)
        {
            if (collision.gameObject.CompareTag("Stalwart"))
            {
                ParticleSystem p = Instantiate(zap);
                p.gameObject.transform.position = collision.contacts[0].point;
                p.Play();
            }
            
            stun.Activate();
            charged = false;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            if (collision.gameObject.transform.position.y + 0.4f < transform.position.y)
            {
                if (!groundTouching.Contains(collision.gameObject))
                {
                    groundTouching.Add(collision.gameObject);
                }
            }
        }
    }

    void DisablePhysics()
    {
        this.GetComponent<Rigidbody>().detectCollisions = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            groundTouching.Remove(collision.gameObject);
        }
    }

}
