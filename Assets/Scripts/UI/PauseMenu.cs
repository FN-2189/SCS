using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private InputManager input;
    public GameObject pauseMenuUI;
    public GameObject quitScreen;
    public GameObject settingsScreen;

    public bool isPaused = false;
    private bool quitScreenActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        input = GameObject.Find("Manager").GetComponent<InputManager>();
        isPaused = false;
        pauseMenuUI.SetActive(false);
        quitScreen.SetActive(false);
        settingsScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Escape)
        {
            if (isPaused)
            {
                Cursor.visible = false;
                Resume();
            }
            else
            {
                Cursor.visible = true;
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        quitScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        quitScreenActive = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Load");
    }

    public void QuitGameScreen()
    {
        quitScreenActive = true;
        quitScreen.SetActive(true);
    }

    public void QuitGameScreenExit()
    {
        quitScreenActive = false;
        quitScreen.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsScreen.SetActive(true);
        settingsScreen.GetComponent<Options>().Setup();
    }
}
