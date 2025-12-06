using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmationDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText; // Or use Text if not using TMP
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TextMeshProUGUI titleText;
    private Action onYes;
    private Action onNo;

    public void Setup(string title, string message, Action yesAction, Action noAction)
    {
        messageText.text = message;
        titleText.text = title;
        onYes = yesAction;
        onNo = noAction;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => { onYes?.Invoke(); Destroy(gameObject); });
        noButton.onClick.AddListener(() => { onNo?.Invoke(); Destroy(gameObject); });
    }
}