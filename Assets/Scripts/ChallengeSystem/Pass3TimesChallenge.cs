using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class Pass3TimesChallenge : Challenge<int>
{
    public int SEQUENCE;
    private int limitOfEachGame;
    //public SavedInteger LIMIT_OF_EACH_GAME;
    public SettingsManager Settings;
    public SavedInteger CURRENT_COUNTER;
    public int INCREASED_DIFFICULTY;
    private int currentCounter;
    private int previusCounter;
    // bool isCompleted = false;
    bool isCompletedPartially = false;

    public UnityEvent<int> OnSetProgressUnnormalized;
    public UnityEvent<string> OnCompletedPartiallyString;
    public UnityEvent<int> OnLimitInitilized;
    public UnityEvent<bool> OnSetIsCompleted;
    public UnityEvent<bool> OnSetIsCompletedPartially;

    //private int lastScore;

    public void Initialize()
    {
        limitOfEachGame = Settings.settings.Pass3ChallengeLimitOfEachGame;
        OnLimitInitilized?.Invoke(limitOfEachGame);
    }

    //called on game over TODO make it called that way.
    public override void CheckIsCompleted(int s)
    {
        //isCompleted = false;
        if (s >= limitOfEachGame)
        {
            currentCounter++;
            previusCounter = currentCounter;
        }
        else //save the counter in case player will purchse hearts to continue the game.
        {
            previusCounter = currentCounter;
            currentCounter = 0;
        }
        OnSetProgressUnnormalized?.Invoke(currentCounter);
        if (currentCounter >= SEQUENCE /*&& !isCompleted*/)
        {
            //isCompleted = true;
            OnSetIsCompleted?.Invoke(true);
            OnCompleted?.Invoke();
            print("You succeeded Pass3TimesChallenge Challenge!!");
            limitOfEachGame += INCREASED_DIFFICULTY;
            currentCounter = 0;
        }
    }

    //called for each addtiotion to score
    public void IsCompletetdPartially(int s)
    {
        if (s >= limitOfEachGame && !isCompletedPartially)
        {
            isCompletedPartially = true;
            OnSetIsCompletedPartially?.Invoke(true);
            OnCompletedPartiallyString?.Invoke((currentCounter+1).ToString()+"|"+SEQUENCE.ToString());
            CURRENT_COUNTER.SaveValue(currentCounter + 1);
            print("Pass3TimesChallenge: " + currentCounter + "/" + SEQUENCE);
        }
    }

    public void SetGameContinues()
    {
        currentCounter = previusCounter;
        //OnSetProgressUnnormalized?.Invoke(currentCounter);
        previusCounter = -1;
    }

    public void SetNewGame()
    {
        isCompletedPartially = false;
        //lastScore = 0;
    }

    public override void ResetProgress()
    {
        currentCounter = 0;
        //isCompleted = false;
        OnSetIsCompleted?.Invoke(false);
    }

    public void Awake()
    {
        currentCounter = 0;
        currentCounter = -1;
        currentCounter = CURRENT_COUNTER.GetValue();
        previusCounter = currentCounter;
    }

}
