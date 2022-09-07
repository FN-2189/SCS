using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject quitScreen;

    private bool quitScreenActive = false;

    // Start is called before the first frame update
    void Start()
    {
        quitScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
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
}
