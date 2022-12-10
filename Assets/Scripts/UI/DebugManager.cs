using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    [SerializeField]
    private LabelID[] labels;

    [SerializeField]
    private GameObject debugSettingsPrefab;

    [SerializeField]
    private Transform debugSettingsMenu;

    private List<DebugSetting> _settings;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Warning: DebugManager already exists!");
            Destroy(gameObject);
        }

        instance = this;

        _settings = new List<DebugSetting>();

        for(int i = 0; i < labels.Length; i++)
        {
            GameObject obj = Instantiate(debugSettingsPrefab, debugSettingsMenu);
            RectTransform t = obj.GetComponent<RectTransform>();
            t.anchoredPosition = Vector3.zero;
            t.localPosition = new Vector3(5f ,540f - 25f - 50f * i, 0f);
            DebugSetting setting = obj.GetComponent<DebugSetting>();
            setting.Setup(labels[i].Label, labels[i].ID);
            _settings.Add(setting);
        }
    }

    private void Update()
    {
        if (InputManager.Debug)
        {
            debugSettingsMenu.gameObject.SetActive(!debugSettingsMenu.gameObject.activeSelf);

            // disable all settings when not displayed
            if (!debugSettingsMenu.gameObject.activeSelf)
            {
                foreach(DebugSetting s in _settings)
                {
                    s.SetState(false);
                }
            }
        }
    }

    public bool GetSettingState(string id)
    {
        DebugSetting setting;
        if((setting = _settings.Find(setting => setting.ID == id)) == null)
        {
            Debug.LogError($"Error: Could not find setting: {id}!");
            return false;
        }
        return _settings.Find(setting => setting.ID == id).GetState();
    }
}

[System.Serializable]
public class LabelID
{
    public string ID;
    public string Label;
}
