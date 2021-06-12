/**
 * Wiggle.cs makes an object move in random directions over time
 * Author:  Lisa Walkosz-Migliacio  http://evilisa.com  09/18/2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour {

    public bool wiggle;
    public float speed;
    Vector2 origin;
    float timeBetween;
    float offset = 1f;

    // Use this for initialization
    void Start () {
        origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (wiggle)
        {
            if (timeBetween > 1f / speed)
            {
                float toX = Random.Range(-1.0f, +1.0f);
                float toY = Random.Range(-1.0f, +1.0f);
                transform.position = new Vector2(origin.x + toX, origin.y + toY);
                timeBetween = 0f;
                if (transform.position.x + offset < origin.x &&
                     transform.position.x - offset > origin.x &&
                     transform.position.y + offset < origin.y &&
                     transform.position.y - offset > origin.y)
                {
                    transform.position = origin;
                }
            }
            else
            {
                timeBetween += Time.deltaTime;
            }
        }
        else
        {
            transform.position = origin;
        }
	}
}
