using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject exitSettingsScreen;
    public GameObject SettingsGraphicsPanel;
    public GameObject SettingsBindingsPanel;
    public GameObject SettingsAudioPanel;
    public GameObject SettingsPanel;

    //Settings
    public Toggle fullscreenToggle, vSyncToggle;
    public bool changedSettings = false;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

    }

    public void Setup()
    {
        exitSettingsScreen.SetActive(false);
        SettingsBindingsPanel.SetActive(false);
        SettingsAudioPanel.SetActive(false);
        SettingsGraphicsPanel.SetActive(true);

        //Setup toggles
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 1)
        {
            vSyncToggle.isOn = true;
        }
        else
        {
            vSyncToggle.isOn = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseSettings()
    {
        if (changedSettings)
        {
            exitSettingsScreen.SetActive(true);
        }
        else
        {
            exitSettingsScreen.SetActive(false);
            SettingsPanel.SetActive(false);
        }
    }

    public void ChangedSettings()
    {
        changedSettings = true;
    }

    public void SettingsGraphics()
    {
        SettingsBindingsPanel.SetActive(false);
        SettingsAudioPanel.SetActive(false);
        SettingsGraphicsPanel.SetActive(true);
    }
    public void SettingsBindings()
    {
        SettingsBindingsPanel.SetActive(true);
        SettingsAudioPanel.SetActive(false);
        SettingsGraphicsPanel.SetActive(false);
    }

    public void ExitSettingsYes()
    {
        exitSettingsScreen.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    public void ExitSettingsNo()
    {
        exitSettingsScreen.SetActive(false);
    }

    public void ApplySettings()
    {
        changedSettings = false;

        Screen.fullScreen = fullscreenToggle.isOn;

        if (vSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        } else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    //Settings
    //Graphics

}
