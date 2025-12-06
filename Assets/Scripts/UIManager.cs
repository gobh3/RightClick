using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public GameObject mainWindow;
    public GameObject buyHeartsWindow;
    public GameObject errorWindow;
    public GameObject settingsWindow;
    public UnityEvent OnBuyHeartsWindowOpened;
    public void ShowMainWindow()
    {
        CloseAll();
        if (mainWindow != null)
            mainWindow.SetActive(true);
        else
            Debug.LogWarning("Main Window is not assigned in UIManager.");
    }

    public void ShowBuyHeartsWindowWindow()
    {
        if (buyHeartsWindow != null)
        {
            buyHeartsWindow.SetActive(true);
            OnBuyHeartsWindowOpened?.Invoke();
        }
        else
            Debug.LogWarning("Buy Hearts Window is not assigned in UIManager.");
    }

    public void CloseBuyHeartsWindowWindow()
    {
        if (buyHeartsWindow != null)
            buyHeartsWindow.SetActive(false);
        else
            Debug.LogWarning("Buy Hearts Window is not assigned in UIManager.");
    }


    public void ShowErrorWindow(string message)
    {
        CloseAll();
        if (errorWindow != null)
        {
            errorWindow.SetActive(true);
            var tmpText = errorWindow.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmpText != null)
                tmpText.text = message;
            else
                Debug.LogWarning("No TextMeshProUGUI found in Error Window.");
        }
        else
        {
            Debug.LogWarning("Error Window is not assigned in UIManager.");
        }
    }

    public void ShowSettingsWindow()
    {
        CloseAll();
        if (settingsWindow != null)
            settingsWindow.SetActive(true);
        else
            Debug.LogWarning("Settings Window is not assigned in UIManager.");
    }

    public void CloseSettingsWindowWindow()
    {
        if (settingsWindow != null)
        {
            settingsWindow.SetActive(false);
            ShowMainWindow();
        }
        else
            Debug.LogWarning("Buy Hearts Window is not assigned in UIManager.");
    }
    private void CloseAll()
    {
        if (mainWindow != null)
            mainWindow.SetActive(false);
        if (buyHeartsWindow != null)
            buyHeartsWindow.SetActive(false);
        if (errorWindow != null)
            errorWindow.SetActive(false);
    }
}
