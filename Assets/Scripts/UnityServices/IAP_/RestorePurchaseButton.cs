using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class RestorePurchaseButton : MonoBehaviour
{
    public GameObject restoreButton;
    public bool ActiveOnIphone;
    public bool ActiveOnAndroid;
    public bool ActiveOnEditor;

    public void Awake()
    {
        restoreButton.SetActive(false);
        if (ActiveOnAndroid && Application.platform == RuntimePlatform.Android)
            restoreButton.SetActive(true);
        else if (ActiveOnIphone && Application.platform == RuntimePlatform.IPhonePlayer)
            restoreButton.SetActive(true);
        else if (ActiveOnEditor && Application.isEditor)
            restoreButton.SetActive(true);
    }
}
