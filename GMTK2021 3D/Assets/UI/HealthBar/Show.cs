using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show : MonoBehaviour {

    public bool show;
    public GameObject HeartFull;
	
    // Use this for initialization
	void Start () {
        show = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (show)
        {
            HeartFull.GetComponent<Image>().enabled = true;
        }
        else
        {
            HeartFull.GetComponent<Image>().enabled = false;
        }
	}
}
