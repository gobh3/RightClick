using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class TextControllerLerp : MonoBehaviour, ITimerClient
{
    private TextMeshProUGUI textBox;

    [Range(0f, 10f)]
    public float LERP_DURATION = 1f;

    private LerpRecord<float?> lerpRecord = new LerpRecord<float?>();

    public bool HasConstantInitialValue = false;
    public UnityEvent OnValueChangedContinuesly;
    public float initialValue;

    private void Awake()
    {
        textBox = GetComponent<TextMeshProUGUI>();
        if (HasConstantInitialValue && lerpRecord != null && lerpRecord.Current.HasValue)
        {
            lerpRecord.To = initialValue;
            if (textBox != null)
                textBox.text = (System.Math.Round(lerpRecord.Current.Value)).ToString();
        }
    }

    public void ChangeValueContinuosly(int score)
    {
        if (lerpRecord != null)
        {
            lerpRecord.To = score;
            AlarmClock.GetInstance()?.RegisterAndReplace(LERP_DURATION, this);
        }
    }
    public void ChangeValue(int score)
    {
        if (lerpRecord != null)
        {
            lerpRecord.Reset(score);
            if (textBox != null && lerpRecord.Current.HasValue)
                textBox.text = (System.Math.Round(lerpRecord.Current.Value)).ToString();
        }
    }

    public void DuringTimer(float elapsedTime)
    {
        if (LERP_DURATION != 0 && lerpRecord != null)
            lerpRecord.UpdateCurrent(elapsedTime / LERP_DURATION, MathUtil.Lerp);

        if (textBox != null && lerpRecord != null && lerpRecord.Current.HasValue)
        {
            string updatedT = (System.Math.Round(lerpRecord.Current.Value)).ToString();
            if (updatedT != textBox.text)
            {
                textBox.text = updatedT;
                OnValueChangedContinuesly?.Invoke();
            }
        }
    }
}
