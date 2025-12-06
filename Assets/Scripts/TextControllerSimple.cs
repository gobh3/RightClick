using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextControllerSimple : MonoBehaviour
{
    private TextMeshProUGUI textBox;

    public UnityEvent OnValueChanged;

    private void Awake()
    {
        textBox = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeValue(string str)
    {
        if (textBox != null)
        {
            textBox.text = str;
            OnValueChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found.");
        }
    }

    public void ChangeValue(int v)
    {
        if (textBox != null)
        {
            textBox.text = v.ToString();
            OnValueChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found.");
        }
    }

    public void ChangeValue(float v)
    {
        if (textBox != null)
        {
            textBox.text = v.ToString();
            OnValueChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found.");
        }
    }
}
