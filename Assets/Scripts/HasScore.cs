using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class HasScore : MonoBehaviour
{
    public int FirstHigscore;
    public bool IgnoreSavedHighscoreOnEditor;
    public UnityEvent<int> OnScoreChanged;
    public UnityEvent<int> OnScoreReset;
    public UnityEvent<int> OnNewHighscore;
    public UnityEvent<int> OnSetHighscore;
    public UnityEvent<int> OnGameoverScoreChanged;
    private int score;
    private int highscore;
    private bool IsHighscoreSurviveFromLastTime = true;
    private string HIGHSCORE_VAR = "highscore";

    public void Initialize()
    {
        if (!PlayerPrefs.HasKey(HIGHSCORE_VAR)) //
        {
            PlayerPrefs.SetInt(HIGHSCORE_VAR, FirstHigscore);
            PlayerPrefs.Save();
        }
        score = 0;
        IsHighscoreSurviveFromLastTime = true;
        if (IgnoreSavedHighscoreOnEditor && Application.isEditor)
            highscore = FirstHigscore;
        else
            highscore = PlayerPrefs.GetInt(HIGHSCORE_VAR);
        OnSetHighscore?.Invoke(highscore);
        OnScoreReset?.Invoke(score);
    }

    public void AddScore(int toAdd = 10)
    {
        score += toAdd;
        OnScoreChanged?.Invoke(score);
        if (score > highscore)
        {
            if (IsHighscoreSurviveFromLastTime)
            {
                IsHighscoreSurviveFromLastTime = false;
                OnNewHighscore?.Invoke(score);
            }
            OnSetHighscore?.Invoke(score);
            highscore = score;
            PlayerPrefs.SetInt(HIGHSCORE_VAR, score);
            PlayerPrefs.Save();
        }

    }

    public int GetScore()
    {
        return score;
    }

    public void Gameover()
    {
        OnGameoverScoreChanged?.Invoke(score);
    }
}
