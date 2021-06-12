using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public InteractionField interaction;
    public bool charged;
    public Stun stun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interaction.isActiveAndEnabled && interaction.player != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interaction.player.GetComponent<PlayerController>().AddPart(gameObject);
                interaction.gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (charged)
        {
            stun.Activate();
            charged = false;
        }
    }
}
