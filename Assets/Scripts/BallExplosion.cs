using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallExplosion : MonoBehaviour
{
    private void Awake()
    {
        SoundManager.GetInstance().PlayBoomSound();
    }
}
