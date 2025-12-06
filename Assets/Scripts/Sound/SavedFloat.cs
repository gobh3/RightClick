using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class SavedFloat : MonoBehaviour
{
    public float defaultValue;
    public string VAR_NAME;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(VAR_NAME))
        {
            PlayerPrefs.SetFloat(VAR_NAME, defaultValue);
            PlayerPrefs.Save();
        }
    }

    public void SaveValue(float v)
    {
        if (PlayerPrefs.HasKey(VAR_NAME))
        {
            PlayerPrefs.SetFloat(VAR_NAME, v);
            PlayerPrefs.Save();
        }
    }

    public float GetValue()
    {
        if (PlayerPrefs.HasKey(VAR_NAME))
        {
            return PlayerPrefs.GetFloat(VAR_NAME);
        }
        return defaultValue;
    }
}
