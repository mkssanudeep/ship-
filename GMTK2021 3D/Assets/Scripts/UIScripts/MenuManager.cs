using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuButtonPanel;
    public GameObject pauseMenuButtonPanel;
    public GameObject gameUIPanel;

    private bool gameHasStarted = false; 

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (gameHasStarted)
        {
            if (Input.GetKey(KeyCode.P))
            {
                GamePaused();
            }
        }
    }

    public void TappedToPlay()
    {
        mainMenuButtonPanel.SetActive(false);
        StartGame();
        gameHasStarted = true; 
    }

    public void StartGame()
    {
        //SceneLoader.instance.LoadLevel("TestScene");
        gameUIPanel.SetActive(true);

    }

    public void GamePaused()
    {
        gameUIPanel.SetActive(false);
        pauseMenuButtonPanel.SetActive(true);
    }
}
