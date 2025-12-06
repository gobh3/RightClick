using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//using UnityEngine.Rendering.UI;

public class Progressbar : MonoBehaviour, ITimerClient
{
    public UnityEvent OnFilledEntirely;
    public UnityEvent OnEmpty;
    public float FILL_DURATION = 1f;
    public Image fill;
    public bool emptyWhenIsFilled = true;

    private LerpRecord<float?> lerpRecord = new LerpRecord<float?>();
    private float balance = 0f;
    private void Awake()
    {
        lerpRecord.To = 0f;
        fill.fillAmount = lerpRecord.Current.Value;
    }

    public void OnFillChanged(float to)
    {
        lerpRecord.To = to;
        if (to > lerpRecord.From)
        {
            AlarmClock.GetInstance().RegisterAndReplace(FILL_DURATION, this);
        }
    }

    public void DuringTimer(float elapsedTime)
    {
        lerpRecord.UpdateCurrent(elapsedTime / FILL_DURATION, MathUtil.Lerp);
        fill.fillAmount = lerpRecord.Current.Value;
        if (lerpRecord.Current == lerpRecord.To && lerpRecord.To >= 1f)
        {
            //AlarmClock.GetInstance().RemoveYourself(this);
            balance = lerpRecord.To.Value - 1f;
            OnFilledEntirely?.Invoke();
            if(emptyWhenIsFilled)
                StartCoroutine(emptyAndContinueFilling());
            //fill.fillAmount = lerpRecord.Current.Value;
        }
    }

    private IEnumerator emptyAndContinueFilling()
    {
        lerpRecord.To = 0f;
        AlarmClock.GetInstance().RegisterAndReplace(FILL_DURATION, this);
        yield return new WaitForSeconds(FILL_DURATION);
        lerpRecord.To = balance;
    }

    public float GetCurrentFillAmount()
    {
        return lerpRecord.Current.Value;
    }

    public void Reset(float init)
    {
        if (init < 0f || init > 1f)
            print("Tried to fill progressbar with illegal fillamount value");
        else
        {
            OnEmpty?.Invoke();
            lerpRecord.Reset(init);
            fill.fillAmount = 0f;
        }
    }
}
