using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spot : MonoBehaviour
{
    private Animator spotAnimator;
    public void Awake()
    {
        bool ok = TryGetComponent<Animator>(out spotAnimator);
        if (!ok)
            print("Didn't find spot Animator on spot prefab");
    }

    public void TurnOn()
    {
        spotAnimator.SetBool("on", true);
    }
    public void TurnOff()
    {
        spotAnimator.SetBool("on", false);
    }
}
