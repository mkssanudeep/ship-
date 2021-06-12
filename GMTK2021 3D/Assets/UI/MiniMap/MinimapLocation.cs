/**
 * MinimapLocation.cs icon for player in minimap rotates and follows the player
 * Author:  Lisa Walkosz-Migliacio  http://evilisa.com  09/18/2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapLocation : MonoBehaviour
{ 
    public Transform player;
    public Image playerIcon;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 position = player.position;
        position.y = transform.position.y;
        transform.position = position;

        playerIcon.transform.rotation = Quaternion.Euler(0f, 0f, -player.eulerAngles.y);
    }
}
