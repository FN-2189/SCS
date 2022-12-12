using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugSetting : MonoBehaviour
{
    public string Label;
    public string ID;

    private TMP_Text textField;
    private Toggle toggle;

    public void Setup(string text, string name)
    {
        Label = text;
        ID = name;
        textField = GetComponentInChildren<TMP_Text>();
        toggle = GetComponentInChildren<Toggle>();
        textField.text = Label;
    }

    public bool GetState()
    {
        return toggle.isOn;
    }

    public void SetState(bool value)
    {
        toggle.isOn = value;
    }
}
