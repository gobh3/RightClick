using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningPopup : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public void Close()
    {
        print("Close Warning Popup");
        gameObject.SetActive(false);
    }
    public void Show(string message)
    {
        gameObject.SetActive(true);
        messageText.text = message;
    }
}
