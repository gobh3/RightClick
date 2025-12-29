using UnityEngine;
using UnityEngine.Events;
public class InformOnNearCompletingChallenge : MonoBehaviour
{
    [Tooltip("The score required to complete the challenge.")] public SettingsManager Settings;

    [Tooltip("The percentage (0-1) at which to invoke the near-complete event.")]
    [Range(0f, 1f)]
    public float nearCompletePercent = 0.9f;

    [Header("Events")]
    public UnityEvent onNearComplete;

    private float destinationScore;
    private bool hasInvoked = false;
    private bool isGameOver = false;
    private float currentScore = 0f;

    // Subscribe to your actual score and game over events here
    public void Initialize()
    {
        destinationScore = Settings.settings.PassLimitOfPointsChallenge;
    }

    public void OnScoreChanged(int newScore)
    {
        currentScore = newScore;
        CheckNearComplete();
    }

    public void OnGameOverChanged(bool gameOver)
    {
        isGameOver = gameOver;
        CheckNearComplete();
    }

    private void CheckNearComplete()
    {
        if (hasInvoked || destinationScore <= 0f)
            return;

        if (isGameOver && currentScore / destinationScore >= nearCompletePercent && currentScore / destinationScore < 1f)
        {
            Debug.Log("Challenge is near completion! " + currentScore / destinationScore + "%");
            hasInvoked = true;
            onNearComplete?.Invoke();
            isGameOver = false;
            currentScore = 0f;
        }
    }
}