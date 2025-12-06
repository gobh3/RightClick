using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialTemplate", menuName = "Scriptable Objects/TutorialTemplate")]
//[CreateAssetMenu(fileName = "TutorialTemplate", menuName = "Tutorial/Message Template")]
public class TutorialTemplate : ScriptableObject
{
    public GameObject panelPrefab;
    [Header("Style Settings")]
    public TMP_FontAsset titleFont;
    public TMP_FontAsset descriptionFont;
    public Color titleColor = Color.white;
    public Color descriptionColor = Color.white;
    public Color backgroundColor = new Color(0, 0, 0, 0.8f);
    public Sprite backgroundSprite;
    public Vector2 panelSize = new Vector2(400, 300);
    [Header("Animation Settings")]
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.3f;
}
