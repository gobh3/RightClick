using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateController : MonoBehaviour
{
    enum GameState
    {
        InGame,
        GameOver
    }

    public bool ALWAYS_FIRST_START_ON_EDITOR = false; // Set to true if you want to always simulate first start in the editor

    public UnityEvent OnApplicationStarts;
    public UnityEvent OnApplicationStartsForTheFirstTime;
    public UnityEvent OnNewGameStart;
    public UnityEvent OnGameContinues;
    public UnityEvent OnGameOver;
    public UnityEvent<int> OnGameOverWithCount; // Event with game count parameter (0 = first game, 1 = second, etc.)
    public UnityEvent OnQuitFromApplication;

    private GameState state = GameState.InGame;

    // Start is called before the first frame update
    void Start()
    {
        // Check if a key "HasLaunchedBefore" exists
        if (!PlayerPrefs.HasKey("HasLaunchedBefore") || (ALWAYS_FIRST_START_ON_EDITOR && Application.isEditor))
        {
            // This is the first time the app runs
            Debug.Log("First time launch!");
            OnApplicationStartsForTheFirstTime?.Invoke();
            // Set the key so next time it's not considered the first launch
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);
            PlayerPrefs.Save(); // Make sure to save the changes
        }
        else
        {
            // Not the first launch
            Debug.Log("App has been launched before.");
        }
        OnApplicationStarts?.Invoke();
        InformNewGameStart();
        state = GameState.InGame;
    }

    public void InformGameOver()
    {
        if (state != GameState.GameOver)
        {
            // Get current game count (0 for first game, 1 for second, etc.)
            int gameCount = PlayerPrefs.GetInt("GameCount", 0);
            
            // Invoke events
            OnGameOver?.Invoke();
            OnGameOverWithCount?.Invoke(gameCount);
            
            // Increment game count for next game
            PlayerPrefs.SetInt("GameCount", gameCount + 1);
            PlayerPrefs.Save();
            
            Debug.Log($"Game Over - Game #{gameCount + 1}");
            state = GameState.GameOver;
        }
        //print("------Game Over------");
    }

    public void InformGameContinues()
    {
        OnGameContinues?.Invoke();
        state = GameState.InGame;
        //print("Game Continues...");
    }

    public void InformNewGameStart()
    {
        OnNewGameStart?.Invoke();
        state = GameState.InGame;
        //print("------New Game------");
    }

    private void OnApplicationQuit()
    {
        OnQuitFromApplication?.Invoke();
    }

    // Helper method to check if user has experienced game over before (useful for other scripts)
    public bool HasUserExperiencedGameOverBefore()
    {
        return PlayerPrefs.GetInt("GameCount", 0) > 0;
    }
    
    // Get current game count
    public int GetGameCount()
    {
        return PlayerPrefs.GetInt("GameCount", 0);
    }

    // Method to reset game count (useful for testing or special cases)
    public void ResetGameCount()
    {
        PlayerPrefs.DeleteKey("GameCount");
        PlayerPrefs.Save();
        Debug.Log("Game count reset to 0.");
    }
}