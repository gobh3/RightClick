using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Challenge<T> : MonoBehaviour//, IChallengeInfo
{
    public UnityEvent OnCompleted;
    
    public abstract void CheckIsCompleted(T arg);
    //public abstract bool IsCompleted();

    //should be in a particular ChalengeInfo interface:
    public abstract void ResetProgress();
    //public abstract int GetDestination(); 
    //public abstract float GetProgress();
   // public abstract int GetUnnormalizedProgress();
    //public abstract int GetInitialValue();
}
