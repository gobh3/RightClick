using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedBool : MonoBehaviour
{
    //private static int index = 0; TODO!!!
    public bool defaultValue;
    public string BASE_VAR_NAME;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(BASE_VAR_NAME))
        {
            PlayerPrefs.SetInt(BASE_VAR_NAME, (defaultValue ? 1 : 0));
            PlayerPrefs.Save();
        }
    }

    public void SaveValue(bool v)
    {
        if (PlayerPrefs.HasKey(BASE_VAR_NAME))
        {
            PlayerPrefs.SetInt(BASE_VAR_NAME, (v ? 1 : 0));
            PlayerPrefs.Save();
        }
    }

    public bool GetValue()
    {
        if (PlayerPrefs.HasKey(BASE_VAR_NAME))
        {
            return 1 == PlayerPrefs.GetInt(BASE_VAR_NAME);
        }
        return defaultValue;
    }
}
