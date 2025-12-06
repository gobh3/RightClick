using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleConfigManager : MonoBehaviour
{
    public SettingsManager settingsManager;
    private ObstacleGameConfig obstacleGameConfig;
    private ObstacleConfig currentConfig;
    public void InitializeObstacleConfigManager()
    {
        if (settingsManager.obstacleGameConfig == null)
        {
            Debug.Log("Using default obstacle config");
            // Initialize with default values if no config exists
            obstacleGameConfig = new ObstacleGameConfig
            {
                defaultConfig = new ObstacleConfig
                {
                    proportions = new Dictionary<ObstacleType, float> {
                        { ObstacleType.ClassicObstacle, 85 },
                        { ObstacleType.CircularObstacle, 10 },
                        { ObstacleType.DoubleObstacle, 5 }
                    }//,
                    //patterns = new List<Dictionary<ObstacleType, int>>()
                },
                levels = new Dictionary<int, ObstacleConfig>()
            };
            currentConfig = obstacleGameConfig.defaultConfig;
        }
        //if there is a remote obstacleGameConfig, set currentConfig to the remote default config.
        else
        {
            obstacleGameConfig = settingsManager.obstacleGameConfig;
            currentConfig = obstacleGameConfig.defaultConfig;
        }
    }

    public void SetConfig(int level)
    {
        ObstacleConfig levelConfig = obstacleGameConfig.levels.ContainsKey(level)
            ? obstacleGameConfig.levels[level]
            : currentConfig;

       currentConfig = levelConfig;
    }

    public ObstacleType GetRandObsType()
    {
        return ChooseObstacleType(currentConfig.proportions);
    }

    private ObstacleType ChooseObstacleType(Dictionary<ObstacleType, float> proportions)
    {
        float total = proportions.Values.Sum();
        float r = UnityEngine.Random.Range(0f, total);
        float cumulative = 0;

        foreach (var kvp in proportions)
        {
            cumulative += kvp.Value;
            if (r <= cumulative)
            {
                return kvp.Key;
            }
        }

        return proportions.Keys.First(); // Fallback to first type
    }

    private List<ObstacleType> GeneratePattern(Dictionary<ObstacleType, int> pattern)
    {
        List<ObstacleType> patternObstacles = new List<ObstacleType>();
        foreach (var kvp in pattern)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                patternObstacles.Add(kvp.Key);
            }
        }
        return patternObstacles;
    }
}