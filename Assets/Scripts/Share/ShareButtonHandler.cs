using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShareButtonHandler : MonoBehaviour
{
    public NativeShare nativeShare;    // Assign in inspector
    public Text notificationText;      // UI Text for showing messages
    public string shareMessage = "Check this out! https://play.google.com/store/apps/details?id=com.DefaultCompany.RightClick";
    public string ALREADY_GOT_REWARD = "You already got the reward this session.";
    private bool hasRewardedThisSession = false;

    public WarningPopup warningPopupPf; //warning popup for showing warning messages Prefab
    public GameObject warningPopupFather;

    public UnityEvent<int> OnOrizeForSharing;
    public SavedInteger countShare;

    // Called by the UI Button OnClick event
    public void OnShareButtonClicked()
    {
        if (!hasRewardedThisSession)
        {
            nativeShare.Share(shareMessage); // Trigger the native share dialog
            countShare.SaveValue(countShare.GetValue() + 1); // Increment share count
            OnOrizeForSharing?.Invoke(countShare.GetValue());
            hasRewardedThisSession = true;
        }
        else
        {
            // Show warning popup
            WarningPopup warningPopup = Instantiate(warningPopupPf, warningPopupFather.transform);
            warningPopup.Show(ALREADY_GOT_REWARD);
        }
    }
    /*
    private void GivePrizeToPlayer()
    {
        // Example prize logic
        Debug.Log("Player rewarded for opening share sheet!");
        ShowNotification("Thanks for sharing! You earned a prize!");
        // TODO: Add actual reward logic here (coins, unlocks, etc)
    }

    private void ShowNotification(string message)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            CancelInvoke(nameof(ClearNotification));
            Invoke(nameof(ClearNotification), 3f);  // clear after 3 seconds
        }
    }

    private void ClearNotification()
    {
        if (notificationText != null)
        {
            notificationText.text = "";
        }
    }*/
}
