using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Analytics;
using UnityEngine.Events;

public class GameAnalytics : MonoBehaviour
{
    private List<string> gameProcessSteps = new List<string>();
    private bool initialized = false;
    private bool collectingData = false;
    private float sessionStartTime;

    // PlayerPrefs key for storing analytics consent
    private const string ANALYTICS_CONSENT_KEY = "AnalyticsConsent";
    private const int CONSENT_NOT_SET = -1;
    private const int CONSENT_DENIED = 0;
    private const int CONSENT_GRANTED = 1;

    //For privacy Policy compliance, we need to ask for consent
    public string TITLE = "Analytics Consent";
    public string MESSAGE = "Do you allow anonymous analytics to be collected to improve the game? You can change this later in settings.";
    public ConfirmationDialog confirmationDialogPrefab;
    public GameObject confirmationDialogFather;
    public UnityEvent<bool> OnAnalyticsActive;
    public float dialogDelaySeconds = 5f; // Delay before showing the dialog

    void Start()
    {
        // Load saved consent preference on start
        LoadAnalyticsConsent();
    }

    public async void Initialize()
    {
        if (!initialized)
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            initialized = true;
            Debug.Log($"Analytics service initialized (data collection: {(collectingData ? "ON" : "OFF")})");

            // Apply the loaded consent setting (don't save again since we just loaded)
            if (collectingData)
            {
                AnalyticsService.Instance.StartDataCollection();
                Debug.Log("Analytics data collection resumed from saved settings.");
            }
        }
    }

    private void LoadAnalyticsConsent()
    {
        int consentValue = PlayerPrefs.GetInt(ANALYTICS_CONSENT_KEY, CONSENT_NOT_SET);

        switch (consentValue)
        {
            case CONSENT_GRANTED:
                collectingData = true;
                Debug.Log("Analytics consent loaded: GRANTED");
                break;
            case CONSENT_DENIED:
                collectingData = false;
                Debug.Log("Analytics consent loaded: DENIED");
                break;
            case CONSENT_NOT_SET:
            default:
                collectingData = false; // Default to false for privacy
                Debug.Log("Analytics consent not set - defaulting to disabled");
                break;
        }

        // Notify listeners about the loaded state
        OnAnalyticsActive.Invoke(collectingData);
    }

    private void SaveAnalyticsConsent(bool consent)
    {
        int consentValue = consent ? CONSENT_GRANTED : CONSENT_DENIED;
        PlayerPrefs.SetInt(ANALYTICS_CONSENT_KEY, consentValue);
        PlayerPrefs.Save(); // Ensure it's written to disk immediately
        Debug.Log($"Analytics consent saved: {(consent ? "GRANTED" : "DENIED")}");
    }

    public bool HasUserSetConsent()
    {
        return PlayerPrefs.GetInt(ANALYTICS_CONSENT_KEY, CONSENT_NOT_SET) != CONSENT_NOT_SET;
    }

    public void ShowAnalyticsConsentDialog(int gameCount)
    {
        // Only show dialog if:
        // 1. Analytics is currently disabled
        // 2. Game count is 0 (first game) or even numbers (2, 4, 6, etc.)
        if (collectingData)
        {
            Debug.Log($"Analytics consent dialog not shown - analytics already active (Game #{gameCount + 1})");
            return;
        }
        
        if (gameCount % 2 != 0)
        {
            Debug.Log($"Analytics consent dialog not shown - not the right game count (Game #{gameCount + 1})");
            return;
        }
        
        // Show dialog after delay
        StartCoroutine(ShowDialogAfterDelay());
    }
    
    private IEnumerator ShowDialogAfterDelay()
    {
        yield return new WaitForSeconds(dialogDelaySeconds);
        
        var dialog = Instantiate(confirmationDialogPrefab, confirmationDialogFather.transform);
        dialog.Setup(
            TITLE,
            MESSAGE,
            () =>
            { // YES
                EnableAnalyticsCollection();
                OnAnalyticsActive.Invoke(collectingData);
            },
            () =>
            { // NO
                DisableAnalyticsCollection();
                OnAnalyticsActive.Invoke(collectingData);
            }
        );
    }

    public void InitalizeToggle(GenericToggleController toggleController)
    {
        toggleController.SetValue(collectingData);
    }

    // Method to change consent from settings (e.g., toggle in settings menu)
    public void SetAnalyticsConsent(bool consent)
    {
        collectingData = consent;
        SaveAnalyticsConsent(consent);

        if (consent)
        {
            EnableAnalyticsCollection();
        }
        else
        {
            DisableAnalyticsCollection();
        }

        OnAnalyticsActive.Invoke(collectingData);
    }

    // Enable analytics data collection (only after player consent)
    public void EnableAnalyticsCollection()
    {
        if (!initialized)
        {
            Debug.LogWarning("EnableAnalyticsCollection called before Initialize().");
            return;
        }

        AnalyticsService.Instance.StartDataCollection();
        collectingData = true;
        SaveAnalyticsConsent(true); // Save the consent
        Debug.Log("Analytics data collection ENABLED.");
    }

    // Disable analytics data collection
    public void DisableAnalyticsCollection()
    {
        collectingData = false;
        AnalyticsService.Instance.StopDataCollection();
        SaveAnalyticsConsent(false); // Save the refusal
        Debug.Log("Analytics data collection DISABLED.");
    }

    // Start a new game session
    public void ResetGameProcess()
    {
        gameProcessSteps.Clear();
        sessionStartTime = Time.realtimeSinceStartup;
    }

    // Helper method to record events with collection check
    private void RecordAnalyticsEvent(Unity.Services.Analytics.Event analyticsEvent, string eventName)
    {
        if (!collectingData)
        {
            Debug.Log($"[Analytics OFF] Event not sent: {eventName}");
            return;
        }

        AnalyticsService.Instance.RecordEvent(analyticsEvent);
        Debug.Log($"Event recorded: {eventName}");
    }

    // ---- Challenge Events ----
    public void TrackPass3TimesChallenge()
    {
        RecordAnalyticsEvent(new Pass3TimesChallengeCompletedEvent(), "Pass3TimesChallengeCompleted");
        AddStep("Pass3TimesCh");
    }

    public void TrackPassLimitOfPointsChallenge()
    {
        RecordAnalyticsEvent(new PassLimitOfPointsCompletedEvent(), "PassLimitOfPointsCompleted");
        AddStep("PassLimitOfPointsCh");
    }

    public void TrackShareChallenge(int count)
    {
        RecordAnalyticsEvent(new ShareCompletedEvent(count), "ShareCompleted");
    }

    // ---- Game Process Purchases ----
    public void TrackPurchase3Hearts()
    {
        RecordAnalyticsEvent(new Purchase3HeartsEvent(), "Purchase3Hearts");
        AddStep("Purchase3Hearts");
    }

    public void TrackPurchase8Hearts()
    {
        RecordAnalyticsEvent(new Purchase8HeartsEvent(), "Purchase8Hearts");
        AddStep("Purchase8Hearts");
    }

    public void TrackNewHighscore(int highscore)
    {
        RecordAnalyticsEvent(new NewHighscoreEvent(highscore), "NewHighscore");
        AddStep("NewHighscore", highscore.ToString());
    }

    // ---- Non-process purchases ----
    public void TrackBuyBonus2Hearts()
    {
        RecordAnalyticsEvent(new BuyBonus2HeartsEvent(), "BuyBonus2Hearts");
    }

    public void TrackBuyPremiumBonus2Hearts()
    {
        RecordAnalyticsEvent(new BuyPremiumBonus2HeartsEvent(), "BuyPremiumBonus2Hearts");
    }

    // ---- Game Process Building with Timing ----
    public void AddStep(string stepName, string detail = null)
    {
        float timeSinceStart = Time.realtimeSinceStartup - sessionStartTime;
        string timeStamp = FormatTimeStamp(timeSinceStart);

        if (!string.IsNullOrEmpty(detail))
            gameProcessSteps.Add($"{timeStamp}-{stepName}:{detail}");
        else
            gameProcessSteps.Add($"{timeStamp}-{stepName}");
    }

    private static string FormatTimeStamp(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60f);
        int sec = Mathf.FloorToInt(seconds % 60f);
        return $"{min:D2}:{sec:D2}";
    }

    // ---- Game Completed ----
    public void MarkGameOverAsStep(int score)
    {
        AddStep("GameOver", score.ToString());
    }

    public void TrackGameCompleted()
    {
        if (gameProcessSteps.Count == 0)
        {
            Debug.Log("GameRecord not sent no steps recorded (probably first game).");
            return;
        }

        string gameProcess = ">" + string.Join(">", gameProcessSteps);

        RecordAnalyticsEvent(new GameRecordEvent(gameProcess, gameProcessSteps.Count), "GameRecord");

        Debug.Log($"GameRecord recorded. Steps: {gameProcess}");
        ResetGameProcess();
    }
}