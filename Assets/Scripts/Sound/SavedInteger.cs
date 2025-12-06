using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SavedInteger : MonoBehaviour
{
    public int defaultValue;
    public string VAR_NAME;
    public UnityEvent<int> OnValueChanged;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(VAR_NAME))
        {
            PlayerPrefs.SetInt(VAR_NAME, defaultValue);
            PlayerPrefs.Save();
        }
    }

    public void SaveValue(int v)
    {
        if (PlayerPrefs.HasKey(VAR_NAME))
        {
            PlayerPrefs.SetInt(VAR_NAME, v);
            PlayerPrefs.Save();
        }
        OnValueChanged?.Invoke(v);
    }

    public int GetValue()
    {
        if (PlayerPrefs.HasKey(VAR_NAME))
        {
            return PlayerPrefs.GetInt(VAR_NAME);
        }
        return defaultValue;
    }
}
