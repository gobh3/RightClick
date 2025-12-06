using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "SettingsScriptableObject", menuName = "Scriptable Objects/SettingsScriptableObject")]
[Serializable]
public class Settings// : ScriptableObject
{
    public float InitialSpeed;
    public int InitialScoreToNextLevel;
    public int MaxScoreToNextLevel;
    public int MaxLevel;
    public float MaxSpeedAdition;
    public float InitialSpace;
    public float MinSpace;
    public float SpaceIncrease;
    public float MaxLowerDeviation;
    public float MaxUpperDeviation;
    public float StopSpeed;
    public float LerpDuration;
    public int Pass3ChallengeLimitOfEachGame;
    public int PassLimitOfPointsChallenge;
}
