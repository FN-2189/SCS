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

    public List<DebugSetting> Settings;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Warning: DebugManager already exists!");
            Destroy(gameObject);
        }

        instance = this;

        Settings = new List<DebugSetting>();

        for(int i = 0; i < labels.Length; i++)
        {
            GameObject obj = Instantiate(debugSettingsPrefab, transform);
            RectTransform t = obj.GetComponent<RectTransform>();
            t.anchoredPosition = Vector3.zero;
            t.localPosition = new Vector3(5f ,540f - 25f - 50f * i, 0f);
            DebugSetting setting = obj.GetComponent<DebugSetting>();
            setting.Setup(labels[i].Label, labels[i].ID);
            Settings.Add(setting);
        }
    }

    public bool GetSettingState(string id)
    {
        return instance.Settings.Find(setting => setting.ID == id).GetState();
    }
}

[System.Serializable]
public class LabelID
{
    public string ID;
    public string Label;
}
