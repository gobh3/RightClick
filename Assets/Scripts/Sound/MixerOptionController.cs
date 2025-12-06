using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerOptionController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string optionName;

    public void setOption(float level)
    {
        audioMixer.SetFloat(optionName, Mathf.Log10(level) * 20f);
    }

}
