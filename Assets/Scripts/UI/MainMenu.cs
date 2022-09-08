using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject quitScreen;
    public GameObject settingsPanel;
    public GameObject exitSettingsScreen;
    public GameObject SettingsGraphicsPanel;
    public GameObject SettingsBindingsPanel;
    public GameObject SettingsAudioPanel;

    private bool quitScreenActive = false;
    private bool settingsPanelAcitve = false;
    public bool changedSettings = false;

    // Start is called before the first frame update
    void Start()
    {
        quitScreen.SetActive(false);
        settingsPanel.SetActive(false);
        exitSettingsScreen.SetActive(false);
        SettingsBindingsPanel.SetActive(false);
        SettingsGraphicsPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Main Menu UI
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


    //Settings UI
    public void OpenSettings()
    {
        settingsPanelAcitve = true;
        settingsPanel.SetActive(true);
        exitSettingsScreen.SetActive(false);
        changedSettings = false;
        settingsPanel.GetComponent<Options>().Setup();
    }

}
