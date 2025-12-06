using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedController : MonoBehaviour, ITimerClient
{
    [NonSerialized]
    public UnityEvent<float> OnSetSpeedFactor;

    private static SpeedController instance;

    public float LERP_DURATION = 0.5f;

    public float STOP_SPEED;

    private LerpRecord<float?> lerpRecord = new LerpRecord<float?>();
    private float speedBeforeStop;

    public void Awake()
    {
        instance = this; 
    }

    public static SpeedController GetInstance()
        { return instance; }

    public void OnNewLevel(LevelData level)
    {
        ChangeSpeedSmoothly(level.speed);
    }

    internal float? GetCurrentSpeed()
    {
        return lerpRecord.Current;
    }

    public void ChangeSpeedSmoothly(float f)
    {
        lerpRecord.To = f;
        AlarmClock.GetInstance().RegisterAndReplace(LERP_DURATION,this);
    }

    public void Stop()
    {
        speedBeforeStop = lerpRecord.Current.Value;
        ChangeSpeedSmoothly(STOP_SPEED);
    }

    public void Continue()
    {
        ChangeSpeedSmoothly(speedBeforeStop);
    }

    public void DuringTimer(float timeElapsed)
    {
        lerpRecord.UpdateCurrent(timeElapsed / LERP_DURATION, MathUtil.Lerp);
    }

    public void RegisterToOnSpeedChanged(UnityAction<float?> updateColor)
    {
        lerpRecord.OnCurrentChanged.AddListener(updateColor);
    }

    
}
