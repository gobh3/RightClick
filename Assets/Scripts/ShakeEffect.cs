using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public SettingsManager settings;
    public float duration_max; 
    public float duration_min; 
    public float magnitude_max; 
    public float magnitude_min;
    public Shaker shaker;

    public void ShakeRandomly()
    {
        float duration = Random.Range(settings.effectsSettings.MinDuration, settings.effectsSettings.MaxDuration);
        float magnitude = Random.Range(settings.effectsSettings.MinMagnitude, settings.effectsSettings.MaxMagnitude);
        shaker.Shake(duration, magnitude);
    }
}
