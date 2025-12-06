using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class LerpRecord<T>
{
    public UnityEvent<T> OnCurrentChanged = new UnityEvent<T>();
    public T From { get; private set; }

    public T Current { get; private set; }

    T _to;
    public T To {
        get => _to;
        set {
            if(Current == null)
            {
                Current = value;
            }
            From = Current;
            _to = value;
            OnCurrentChanged?.Invoke(value);
        } 
    }

    internal void JumpTo(T dest)
    {
        From = dest;
        Current = dest;
    }
    internal void Reset(T dest)
    {
        From = dest;
        Current = dest;
        To = dest;
    }
    public void UpdateCurrent(float fraction, Func<T, T, float, T> LerpMethod)
    {
        Current = LerpMethod(From, To, fraction);
        OnCurrentChanged?.Invoke(Current);
    }
}
