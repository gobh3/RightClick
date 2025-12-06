using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObstacleTutorial
{
    public string obstacleId;
    public string title;
    public string description;
    public Sprite obstaclePreview;
    public TutorialTemplate template;
    public UnityEvent onTutorialComplete;
}
