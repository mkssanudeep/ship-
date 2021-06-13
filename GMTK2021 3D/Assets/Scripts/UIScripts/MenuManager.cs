using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuButtonPanel;
    public GameObject pauseMenuButtonPanel;
    public GameObject gameUIPanel;

    public GameObject backGround;
    public GameObject loadingDot1;
    public GameObject loadingDot2;
    public GameObject loadingDot3;
    public GameObject shuttingDown;

    private bool gameHasStarted = false; 

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartupGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            GamePaused();
        }
    }

    IEnumerator StartupGame()
    {
        yield return new WaitForSeconds(1.5f);
        mainMenuButtonPanel.SetActive(true);

    }
    public void TappedToPlay()
    {
        mainMenuButtonPanel.SetActive(false);
        StartCoroutine(Loading());
        StartGame();
        gameHasStarted = true; 
    }

    IEnumerator Loading()
    {
        backGround.GetComponent<Animator>().Play("FadeOut");
        loadingDot1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        loadingDot2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        loadingDot3.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        loadingDot3.SetActive(false);
        loadingDot1.SetActive(false);
        loadingDot2.SetActive(false);
        backGround.GetComponent<Animator>().Play("FadeIn");

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
        //StartCoroutine(Loading());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator GameOver()
    {
        Time.timeScale = 0;
        backGround.SetActive(true);
        shuttingDown.SetActive(true);
        loadingDot1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        loadingDot2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        loadingDot3.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        loadingDot3.SetActive(false);
        loadingDot2.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        loadingDot2.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        shuttingDown.SetActive(false);
        loadingDot1.SetActive(false);
        loadingDot2.SetActive(false);
        loadingDot3.SetActive(false);
        BackToMainMenu();

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
        Application.Quit();
    }
}
