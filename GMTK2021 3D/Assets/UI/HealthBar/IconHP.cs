using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconHP : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float fillAmount;

    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;
    public GameObject hp4;
    public GameObject hp5;
    public GameObject hp6;
    public GameObject hp7;
    public GameObject hp8;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (fillAmount >= 1f / 8 * 8) hp8.GetComponent<Show>().show = true;
        else hp8.GetComponent<Show>().show = false;
        if (fillAmount >= 1f / 8 * 7) hp7.GetComponent<Show>().show = true;
        else hp7.GetComponent<Show>().show = false;
        if (fillAmount >= 1f / 8 * 6) hp6.GetComponent<Show>().show = true;
        else hp6.GetComponent<Show>().show = false;
        if (fillAmount >= 1f / 8 * 5) hp5.GetComponent<Show>().show = true;
        else hp5.GetComponent<Show>().show = false;
        if (fillAmount >= 1f / 8 * 4) hp4.GetComponent<Show>().show = true;
        else hp4.GetComponent<Show>().show = false;
        if (fillAmount >= 1f / 8 * 3) hp3.GetComponent<Show>().show = true;
        else hp3.GetComponent<Show>().show = false;
        if (fillAmount >= 1f / 8 * 2) hp2.GetComponent<Show>().show = true;
        else hp2.GetComponent<Show>().show = false;
        if (fillAmount >= 1f / 8 * 1) hp1.GetComponent<Show>().show = true;
        else hp1.GetComponent<Show>().show = false;
    }
}
