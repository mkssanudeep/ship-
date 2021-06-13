/**
 * DialogueControl.cs have a conversation with two players on the bottom of the screen
 * Author:  Lisa Walkosz-Migliacio  http://evilisa.com  09/18/2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueControl : MonoBehaviour {
    public enum profile
    {
        dwarf,
        dummy
    }

    public class Chat {
        public string text;
        public string nameProfile;
        public profile profile;

        public Chat(string myText, string myName, profile myProfile)
        {
            text = myText;
            nameProfile = myName;
            profile = myProfile;
        }
    }

    public Image leftProfile;
    public Image rightProfile;
    public TextMeshProUGUI text;
    public TextMeshProUGUI nameProfile;
    public Sprite dwarfImage;
    public Sprite dummyImage;
    public Sprite noneImage;
    int currentDialogue;

    List<Chat> speechText = new List<Chat>{
        new Chat("A conversation between two people is hardly that difficult. Maybe we should try this more often?", "Dwarf", profile.dwarf),
        new Chat("However, I can see how it could be tricky, I wouldn't lie about that.", "Dwarf", profile.dwarf),
        new Chat("Don't you want to show off a button <sprite name=\"Input_13\">?", "Dummy", profile.dummy),
        new Chat("Have at you, what a miserable pile of secrets.", "Dummy", profile.dummy)
    };
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space"))
        {
            // show next dialogue
            if (currentDialogue < speechText.Count)
            {
                text.text = speechText[currentDialogue].text;
                nameProfile.text = speechText[currentDialogue].nameProfile;
                if (speechText[currentDialogue].profile == profile.dwarf)
                {
                    // put left side dwarf picture
                    leftProfile.sprite = dwarfImage;
                    rightProfile.sprite = noneImage;
                }
                else if (speechText[currentDialogue].profile == profile.dummy)
                {
                    // put right side dummy picture
                    leftProfile.sprite = noneImage;
                    rightProfile.sprite = dummyImage;
                }
                else
                {
                    // no one is chatting
                }

                currentDialogue++;
            }
            else
            {
                // remove dialogue box

            }
        }
	}
}
