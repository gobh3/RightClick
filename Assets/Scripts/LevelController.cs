using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;
using UnityEngine.Rendering;
//using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class LevelController : MonoBehaviour
{
    public SettingsManager settings;
    public UnityEvent<LevelData> OnLevelChanged;
    public UnityEvent<LevelData> OnFirstLevel;
    
    public AnimationCurve SCORE_TO_NEXT_CHANGE_STYLE;
    public AnimationCurve SPEED_STYLE;
    /*
    [Range(1, 100)]
    public int INITIAL_SCORE_TO_NEXT_LEVEL;


    public int MAX_SCORE_TO_NEXT_LEVEL;
    public int MAX_LEVEL;
    [Range(1,100)]
    public float MAX_SPEED_ADDITION;
    */
    private LevelData[] levels;

    private LevelData level;


    private void generateLevels()
    {
        levels = new LevelData[settings.settings.MaxLevel];
        //string report = "";
        //report += "speeds report:\n";
        for (int i = 0; i < settings.settings.MaxLevel; i++)
        {
            float newSpeedAddition = SPEED_STYLE.Evaluate((float)i / settings.settings.MaxLevel) * settings.settings.MaxSpeedAdition;
            int newScoreAddition = settings.settings.InitialScoreToNextLevel +
                RoundToTen((float)SCORE_TO_NEXT_CHANGE_STYLE.Evaluate((float)i / settings.settings.MaxLevel) * settings.settings.MaxScoreToNextLevel);
            levels[i] = new LevelData()
            {
                levelNumber = i,
                colorSet = ColorFactory.GetRandomColorAPI(),
                nextLevelAt = (i > 0 ? levels[i-1].nextLevelAt:0) + newScoreAddition,
                speed = newSpeedAddition
            };
            //report += i + ": " + levels[i].speed + ", " + levels[i].nextLevelAt + "(" + newScoreAddition+ ")"+"\n";
        }
        //print(report);
    }

    public void CheckForLevelUp(int score)
    {
        if (level != null && score >= level.nextLevelAt && hasNextLevel())
        {
            level = levels[level.levelNumber + 1];
            //p/rint((level.levelNumber + 1).ToString());
            OnLevelChanged?.Invoke(level);
            //p/rint("You've reacehd level " + level.levelNumber);
        }

        else if(level != null && score >= level.nextLevelAt && !hasNextLevel())
        {
            level = new LevelData()
            {
                levelNumber = level.levelNumber + 1,
                speed = level.speed * 1.1f,
                colorSet = ColorFactory.GetRandomColorAPI(),
                nextLevelAt = level.nextLevelAt + 500
            };
            OnLevelChanged?.Invoke(level);
        }
    }

    private bool hasNextLevel()
    {
        if (level != null && levels.Length > level.levelNumber + 1)
            return true;
        return false;
    }
    public void Initialize()
    {
        generateLevels();  // do it each time so colors will be in diffrent order.
        level = levels[0];
        OnFirstLevel?.Invoke(level);
    }

    private int RoundToTen(float n)
    {
        if (n % 10 > 0)
            return (int)(n - n % 10 + 10);
        else 
            return (int)(n + n % 10 + 10);
    }
}
