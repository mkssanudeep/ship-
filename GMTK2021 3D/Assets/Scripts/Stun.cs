using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public StunField stunField;
    public float stunCooldown;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        //When the player presses space, tell all objects in the stunfield to be stunned
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timer < 0)
            {
                Debug.Log("stunning");
                timer = stunCooldown;
                foreach (StalwartController s in stunField.objectsInField)
                {
                    s.Stun();
                }
            }
        }
    }
}
