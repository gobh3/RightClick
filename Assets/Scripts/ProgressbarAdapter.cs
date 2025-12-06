using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressbarAdapter : MonoBehaviour
{
    public UnityEvent<float> OnProgress;
    public UnityEvent<float> OnReset;

    private int prevNextLevelAt = 0;
    public int total = 0;
    private int progress;

    public void OnProgressChanged(int p)
    {
        progress = p;
        if(total > 0)
            OnProgress?.Invoke((float)(p - prevNextLevelAt) / total);
    }

    public void Reset(int init)
    {
        progress = 0;
        OnReset?.Invoke((float)init); 
    }

    public void OnLevelChanged(LevelData levelData)
    {
        prevNextLevelAt = progress;
        total = levelData.nextLevelAt-progress;
    }
}
