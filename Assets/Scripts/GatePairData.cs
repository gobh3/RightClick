using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeySide
{
    Left, Right
}

public class GatePairData : MonoBehaviour
{
    public KeySide Key { get; private set; }
    public KeySide Other { get; private set; }
    public bool IsInversed { get; private set; }

    private bool isMissed;
    public void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        int a = Random.Range(0, 2);
        Key = KeySide.Left;
        Other = KeySide.Right;
        if (a == 0) //TODO: change it to: if (a==0)
        {
            KeySide tmp = Key;
            Key = Other;
            Other = tmp;
        }
        isMissed = true;
        IsInversed = (Random.value > 0.5f);
    }

    public KeySide GetRightSideToShoot()
    {
        if (((Key == KeySide.Left) && !IsInversed)
            || ((Key == KeySide.Right) && IsInversed)) {
            return KeySide.Left;
        }
        else return KeySide.Right;
    }

    internal void SetIsMissed(bool v)
    {
        isMissed = v;
    }

    internal bool GetIsMissed()
    {
        return isMissed;
    }
}
