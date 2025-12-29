using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public GameObject mainWindow;
    public GameObject buyHeartsWindow;
    public GameObject errorWindow;
    public GameObject settingsWindow;
    public UnityEvent OnBuyHeartsWindowOpened;
    public ShabbatDetector shabbatDetector;
    public WarningPopup warningPopupPf; //warning popup for showing warning messages Prefab
    public GameObject warningPopupFather;
    public TabManager tabManager;
    public string CANT_BUY_ON_SHABAT_WARNING;
    public void ShowMainWindow()
    {
        CloseAll();
        if (mainWindow != null)
            mainWindow.SetActive(true);
        else
            Debug.LogWarning("Main Window is not assigned in UIManager.");
    }

    public void ShowBuyHeartsWindowWindowFromButton()
    {
        if(shabbatDetector != null && shabbatDetector.IsShabbat)
        {
            Debug.Log("Can't open store on shabbat");
            // Show warning popup
            WarningPopup warningPopup = Instantiate(warningPopupPf, warningPopupFather.transform);
            warningPopup.Show(CANT_BUY_ON_SHABAT_WARNING);
            return;
        }
        if (buyHeartsWindow != null)
        {
            buyHeartsWindow.SetActive(true);
            OnBuyHeartsWindowOpened?.Invoke();
        }
        else
            Debug.LogWarning("Buy Hearts Window is not assigned in UIManager.");
    }

    public void ShowBuyHeartsWindowFromCode()
    {
        if (shabbatDetector != null && shabbatDetector.IsShabbat)
        {
            return;
        }
        if (buyHeartsWindow != null)
        {
            buyHeartsWindow.SetActive(true);
            tabManager.SwitchTab(0);
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
