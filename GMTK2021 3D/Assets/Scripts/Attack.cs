using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool active;
    private float timer;
    public float windUp;
    public int damage;
    public float coolDown;

    // Start is called before the first frame update
    void Start()
    {
    }

    public bool Ready
    {
        get
        {
            return timer < 0;
        }
    }

    public void StartWindUp()
    {
        timer = windUp;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < -coolDown)
        {
            active = true;
            timer = windUp;
        }
    }
}
