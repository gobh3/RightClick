using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerOptionController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string optionName;
    public float defaultValue;

    private string LAST_VALUE = "LAST_VALUE";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(LAST_VALUE))
        {
            PlayerPrefs.SetFloat(LAST_VALUE, defaultValue);
            PlayerPrefs.Save();
        }
    }
    private void InitailizeOptionWithSavedOrDefault()
    {
    }
    public void setOption(float level)
    {
        audioMixer.SetFloat(optionName, Mathf.Log10(level) * 20f);
    }

}
