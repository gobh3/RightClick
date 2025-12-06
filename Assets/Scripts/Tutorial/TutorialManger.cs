using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

#region Tutorial Data Structures


public class TutorialManager : MonoBehaviour
{
    #region Events
    [Header("Tutorial Events")]
    public UnityEvent onTutorialBegin = new UnityEvent();
    public UnityEvent onTutorialEnd = new UnityEvent();
    public UnityEvent onTutorialStepBegin = new UnityEvent();
    public UnityEvent onTutorialStepComplete = new UnityEvent();
    public UnityEvent onInputStepBegin = new UnityEvent();  // For steps requiring player input
    public UnityEvent onInputStepComplete = new UnityEvent();

    [Header("Obstacle Tutorial Events")]
    public UnityEvent onObstacleTutorialBegin = new UnityEvent();
    public UnityEvent onObstacleTutorialEnd = new UnityEvent();
    #endregion
    #region Tutorial Components
    [Header("Basic Tutorial Settings")]
    [SerializeField] private GameObject basicTutorialPanel;
    [SerializeField] private TextMeshProUGUI basicTitleText;
    [SerializeField] private TextMeshProUGUI basicDescriptionText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private TutorialStep[] tutorialSteps;
    [SerializeField] private float highlightOutlineWidth = 2f;
    [SerializeField] private Color highlightColor = Color.yellow;

    [Header("Obstacle Tutorial Settings")]
    [SerializeField] private TutorialTemplate defaultTemplate;
    [SerializeField] private ObstacleTutorial[] obstacleTutorials;
    #endregion

    #region Private Variables
    private int currentStepIndex = 0;
    private bool isBasicTutorialActive = false;
    private GameObject currentHighlight;
    private Dictionary<string, ObstacleTutorial> tutorialLookup;
    private HashSet<string> completedTutorials;
    private GameObject currentObstacleTutorialPanel;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        InitializeBasicTutorial();
        InitializeObstacleTutorials();
    }

    private void Update()
    {
        // Optional: Add keyboard shortcuts
        if (isBasicTutorialActive && Input.GetKeyDown(KeyCode.Escape))
        {
            EndBasicTutorial();
        }
    }
    #endregion

    #region Initialization
    private void InitializeBasicTutorial()
    {
        nextButton.onClick.AddListener(NextStep);
        previousButton.onClick.AddListener(PreviousStep);
        skipButton.onClick.AddListener(EndBasicTutorial);
    }

    private void InitializeObstacleTutorials()
    {
        tutorialLookup = new Dictionary<string, ObstacleTutorial>();
        foreach (var tutorial in obstacleTutorials)
        {
            tutorialLookup[tutorial.obstacleId] = tutorial;
        }

        completedTutorials = new HashSet<string>();
        LoadCompletedTutorials();
    }
    #endregion

    #region Basic Tutorial Methods
    public void StartBasicTutorial()
    {
        onTutorialBegin?.Invoke();

        isBasicTutorialActive = true;
        currentStepIndex = 0;
        basicTutorialPanel.SetActive(true);
        ShowCurrentStep();
    }

    public void EndBasicTutorial()
    {
        isBasicTutorialActive = false;
        basicTutorialPanel.SetActive(false);
        RemoveHighlight();

        onTutorialEnd?.Invoke();

        PlayerPrefs.SetInt("BasicTutorialCompleted", 1);
        PlayerPrefs.Save();
    }

    private void ShowCurrentStep()
    {
        if (currentStepIndex >= tutorialSteps.Length) return;

        TutorialStep step = tutorialSteps[currentStepIndex];

        onTutorialStepBegin?.Invoke();

        basicTitleText.text = step.title;
        basicDescriptionText.text = step.description;

        previousButton.interactable = currentStepIndex > 0;
        nextButton.interactable = currentStepIndex < tutorialSteps.Length - 1 || !step.waitForInput;

        RemoveHighlight();
        if (step.highlightObject != null)
        {
            HighlightObject(step.highlightObject);
        }

        if (step.waitForInput)
        {
            onInputStepBegin?.Invoke();
            StartCoroutine(WaitForInput(step.requiredInput));
        }
    }

    private void NextStep()
    {
        onTutorialStepComplete?.Invoke();

        if (currentStepIndex < tutorialSteps.Length - 1)
        {
            currentStepIndex++;
            ShowCurrentStep();
        }
        else
        {
            EndBasicTutorial();
        }
    }

    private IEnumerator WaitForInput(string requiredInput)
    {
        while (!Input.GetButtonDown(requiredInput))
        {
            yield return null;
        }

        onInputStepComplete?.Invoke();
        NextStep();
    }

    private void PreviousStep()
    {
        if (currentStepIndex > 0)
        {
            currentStepIndex--;
            ShowCurrentStep();
        }
    }

    private void HighlightObject(GameObject obj)
    {
        currentHighlight = new GameObject("Highlight");
        currentHighlight.transform.position = obj.transform.position;

        var outline = currentHighlight.AddComponent<SpriteRenderer>();
        outline.sprite = obj.GetComponent<SpriteRenderer>()?.sprite;
        outline.color = highlightColor;
        outline.transform.localScale = obj.transform.localScale * 1.1f;
    }

    private void RemoveHighlight()
    {
        if (currentHighlight != null)
        {
            Destroy(currentHighlight);
        }
    }
    #endregion

    #region Obstacle Tutorial Methods
    public void ShowObstacleTutorial(string obstacleId)
    {
        if (completedTutorials.Contains(obstacleId)) return;

        if (tutorialLookup.TryGetValue(obstacleId, out ObstacleTutorial tutorial))
        {
            onObstacleTutorialBegin?.Invoke();
            ShowObstacleTutorialPanel(tutorial);
        }
    }

    private void ShowObstacleTutorialPanel(ObstacleTutorial tutorial)
    {
        TutorialTemplate template = tutorial.template ?? defaultTemplate;

        if (currentObstacleTutorialPanel != null) Destroy(currentObstacleTutorialPanel);
        currentObstacleTutorialPanel = Instantiate(template.panelPrefab, transform);

        ConfigureObstaclePanelFromTemplate(currentObstacleTutorialPanel, template, tutorial);
        StartCoroutine(AnimatePanelIn(currentObstacleTutorialPanel, template.fadeInDuration));
    }

    private void ConfigureObstaclePanelFromTemplate(GameObject panel, TutorialTemplate template, ObstacleTutorial tutorial)
    {
        // Panel setup
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = template.panelSize;

        // Background setup
        Image backgroundImage = panel.GetComponent<Image>();
        if (backgroundImage != null)
        {
            backgroundImage.sprite = template.backgroundSprite;
            backgroundImage.color = template.backgroundColor;
        }

        // Configure texts
        SetupTextComponent(panel, "Title", template.titleFont, template.titleColor, tutorial.title);
        SetupTextComponent(panel, "Description", template.descriptionFont, template.descriptionColor, tutorial.description);

        // Preview image
        if (tutorial.obstaclePreview != null)
        {
            var previewImage = panel.GetComponentInChildren<Image>(true);
            if (previewImage != null && previewImage.gameObject.name.Contains("Preview"))
            {
                previewImage.sprite = tutorial.obstaclePreview;
            }
        }

        // Continue button
        Button continueButton = panel.GetComponentInChildren<Button>(true);
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(() => CompleteObstacleTutorial(tutorial));
        }
    }

    private void SetupTextComponent(GameObject panel, string componentNameContains, TMP_FontAsset font, Color color, string text)
    {
        var tmpComponent = panel.GetComponentsInChildren<TextMeshProUGUI>(true)
            .FirstOrDefault(tmp => tmp.gameObject.name.Contains(componentNameContains));

        if (tmpComponent != null)
        {
            tmpComponent.font = font;
            tmpComponent.color = color;
            tmpComponent.text = text;
        }
    }

    private void CompleteObstacleTutorial(ObstacleTutorial tutorial)
    {
        completedTutorials.Add(tutorial.obstacleId);
        SaveCompletedTutorials();

        StartCoroutine(AnimatePanelOut(currentObstacleTutorialPanel, defaultTemplate.fadeOutDuration));

        onObstacleTutorialEnd?.Invoke();
        tutorial.onTutorialComplete?.Invoke();  // Individual obstacle tutorial events
    }
    #endregion

    #region Save/Load & Animation Methods
    private void LoadCompletedTutorials()
    {
        string completed = PlayerPrefs.GetString("CompletedTutorials", "");
        if (!string.IsNullOrEmpty(completed))
        {
            foreach (string tutorial in completed.Split(','))
            {
                completedTutorials.Add(tutorial);
            }
        }
    }

    private void SaveCompletedTutorials()
    {
        PlayerPrefs.SetString("CompletedTutorials", string.Join(",", completedTutorials));
        PlayerPrefs.Save();
    }

    private IEnumerator AnimatePanelIn(GameObject panel, float duration)
    {
        var canvasGroup = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator AnimatePanelOut(GameObject panel, float duration)
    {
        var canvasGroup = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }
        Destroy(panel);
    }
    #endregion
    
    #endregion
    
}
