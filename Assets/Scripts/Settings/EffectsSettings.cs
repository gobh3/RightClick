using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "EffectsSettings", menuName = "Scriptable Objects/EffectsSettings")]
[Serializable]
public class EffectsSettings// : ScriptableObject
{
    public float MaxDuration;
    public float MinDuration;
    public float MaxMagnitude;
    public float MinMagnitude;
    public int HowManyGatesToKill;
    public float DelayTimeBefore;
    public float DelayTimeAfter;
    public float TimeBetweenGates;

}
