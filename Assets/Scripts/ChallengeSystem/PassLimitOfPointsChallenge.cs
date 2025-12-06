using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class PassLimitOfPointsChallenge : Challenge<int>
{
    public int limit;
    //public SavedInteger LIMIT;
    public SettingsManager Settings;
    public int INCREASE_DIFFICULTY;
    int lastScore = 0;
    bool isCompleted = false;
    public UnityEvent<double> OnSetProgress;
    public UnityEvent<int> OnDestInitilized;
    public UnityEvent<int> OnSetDest;
    public UnityEvent<int> OnSetInitialVal;
    public UnityEvent<bool> OnSetIsCompleted;

    public void Initialize()
    {
        limit = Settings.settings.PassLimitOfPointsChallenge;
        OnDestInitilized?.Invoke(limit);
        //Debug.Log("Limit Initialized: " +  limit);
    }

    public void Inform()
    {
        OnSetDest?.Invoke(limit);
        OnSetInitialVal?.Invoke(0);
        OnSetIsCompleted?.Invoke(false);
    }
    public override void CheckIsCompleted(int score)
    {
        lastScore = score;
        if (score >= limit && isCompleted == false)
        {
            isCompleted = true;
            OnCompleted?.Invoke();
            OnSetIsCompleted?.Invoke(true);
            //p/rint("You succeeded PassLimitOfPoints Challenge!!");
            // make the challenge mroe difficult
            lastScore = 0;
            limit += INCREASE_DIFFICULTY;
            OnSetDest?.Invoke(limit);
        }
        OnSetProgress?.Invoke(getProgress());
    }

    public override void ResetProgress()
    {
        isCompleted = false;
        OnSetIsCompleted?.Invoke(false);
        lastScore = 0;
    }
    /*
    public override int GetDestination()
    {
        return LIMIT;
    }*/

    private double getProgress()
    {
        if (isCompleted) //essential to avoid draw empty progress bar after completing challenge.
            return 1f;
        double progress = Math.Min((double)lastScore / limit,1f);
        return progress;
    }
/*
    public override int GetInitialValue()
    {
        return 0;
    }*/
/*
    public override bool IsCompleted()
    {
        return isCompleted;
    }*/
}
