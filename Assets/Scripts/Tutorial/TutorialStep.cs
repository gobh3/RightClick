using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public string title;
    public string description;
    public GameObject highlightObject;
    public bool waitForInput;
    public string requiredInput;
}