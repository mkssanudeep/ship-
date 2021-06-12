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
    }

    public void Activate()
    {
        if (timer < 0)
        {
            timer = stunCooldown;
            foreach (StalwartController s in stunField.objectsInField)
            {
                s.Stun();
            }
        }
    }
}
