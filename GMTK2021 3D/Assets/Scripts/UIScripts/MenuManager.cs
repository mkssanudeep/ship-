using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //SceneManager.LoadScene("Level 1");
        gameUIPanel.SetActive(true);

    }

    public void GamePaused()
    {
        gameUIPanel.SetActive(false);
        pauseMenuButtonPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    //PAUSE UI BUTTONS

    public void ContinueGame()
    {
        gameUIPanel.SetActive(true);
        pauseMenuButtonPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void BackToMainMenu()
    {
        gameHasStarted = false; 
        gameUIPanel.SetActive(false);
        pauseMenuButtonPanel.SetActive(false);
        mainMenuButtonPanel.SetActive(true);
        //SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {

    }
}
