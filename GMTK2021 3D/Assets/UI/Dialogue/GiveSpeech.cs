/**
 * GiveSpeech.cs speech bubble appears with some text over character
 * Author:  Lisa Walkosz-Migliacio  http://evilisa.com  09/18/2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveSpeech : MonoBehaviour
{
    public Text textbox;
    public GameObject bubble;
 
    List<string> speechText = new List<string>{
        "The first thing I'd say is that this is not all that difficult.",
        "However, I can see how it could be tricky, I wouldn't lie about that.",
        "Have at you, what a miserable pile of secrets."
    };

    // Use this for initialization
    void Start()
    {
        HideSpeechBubble();
        StartCoroutine(SpeechBubbleKickoff());
    }

    IEnumerator SpeechBubbleKickoff()
    {
        foreach (string text in speechText)
        {
            yield return new WaitForSeconds(1f);
            ShowSpeechBubble();
            textbox.text = text;
            yield return new WaitForSeconds(5f);
            HideSpeechBubble();
        }
    }

    void ShowSpeechBubble()
    {
        bubble.SetActive(true);
    }

    void HideSpeechBubble()
    {
        bubble.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
