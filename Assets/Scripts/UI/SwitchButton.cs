using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class SwitchButton : MonoBehaviour
{
    // References to UI elements
    public Image backgroundImage; // Background image
    public Image handleImage; // Handle image
    public TextMeshProUGUI stateText; // Text display
    
    // Material references for ON mode
    public Material backgroundOnMaterial; // Material for background when ON
    public Material handleOnMaterial; // Material for handle when ON
    public Material textOnMaterial; // Material for text when ON
    
    // Variables
    private RectTransform handleRect; // Reference to the handle's RectTransform
    private bool isOn = false; // Current state of the toggle
    
    // OFF mode materials (stored from initial setup in editor)
    private Material backgroundOffMaterial;
    private Material handleOffMaterial;
    private Material textOffMaterial;
    
    // Temporary materials for lerping
    private Material backgroundLerpMaterial;
    private Material handleLerpMaterial;
    private Material textLerpMaterial;
    
    // Lerp progress (0 = OFF, 1 = ON)
    private float lerpProgress = 0f;
    
    // Transition speed
    public float transitionSpeed = 5f;
    
    // Unity Events
    public UnityEvent onToggleOn; // Invoked when toggle switches to ON
    public UnityEvent onToggleOff; // Invoked when toggle switches to OFF
    private void Start()
    {
        // Get handle RectTransform component
        handleRect = handleImage.GetComponent<RectTransform>();
        
        // Store the original OFF materials from the editor setup
        if (backgroundImage != null)
        {
            backgroundOffMaterial = backgroundImage.material;
            // Create a copy for lerping to avoid modifying the original
            backgroundLerpMaterial = new Material(backgroundOffMaterial);
        }
        
        if (handleImage != null)
        {
            handleOffMaterial = handleImage.material;
            // Create a copy for lerping to avoid modifying the original
            handleLerpMaterial = new Material(handleOffMaterial);
        }
        
        if (stateText != null)
        {
            textOffMaterial = stateText.fontSharedMaterial;
            // Create a copy for lerping to avoid modifying the original
            textLerpMaterial = new Material(textOffMaterial);
        }
        
        // Set initial lerp progress based on initial state
        lerpProgress = isOn ? 1f : 0f;
        
        // Update initial handle position and state text
        UpdateHandlePosition();
        UpdateStateText();
        
        // Apply initial materials
        ApplyMaterials();
    }

    public void Toggle()
    {
        isOn = !isOn; // Toggle the state
        UpdateStateText(); // Update state text display
        
        // Invoke appropriate event
        if (isOn)
        {
            onToggleOn?.Invoke();
        }
        else
        {
            onToggleOff?.Invoke();
        }
    }
    
    /// <summary>
    /// Set the toggle value from code. Updates graphics based on the new value.
    /// </summary>
    /// <param name="value">True for ON, False for OFF</param>
    public void SetValue(bool value)
    {
        bool previousValue = isOn;
        isOn = value;
        UpdateStateText();
        
        // Invoke event only if the value actually changed
        if (previousValue != isOn)
        {
            if (isOn)
            {
                onToggleOn?.Invoke();
            }
            else
            {
                onToggleOff?.Invoke();
            }
        }
    }

    private void Update()
    {
        // Determine target lerp progress (0 = OFF, 1 = ON)
        float targetLerpProgress = isOn ? 1f : 0f;
        
        // Smoothly transition lerp progress
        lerpProgress = Mathf.Lerp(lerpProgress, targetLerpProgress, Time.deltaTime * transitionSpeed);
        
        // Update handle position
        UpdateHandlePosition();
        
        // Update materials based on lerp progress
        ApplyMaterials();
    }

    private void UpdateHandlePosition()
    {
        // Calculate target position for handle (accounting for handle width)
        float handleWidth = handleRect.rect.width;
        float backgroundWidth = backgroundImage.rectTransform.rect.width;
        
        // Use lerp progress to determine position
        float targetPosX = lerpProgress * (backgroundWidth - handleWidth);
        
        // Apply position
        handleRect.anchoredPosition = new Vector2(targetPosX, handleRect.anchoredPosition.y);
    }

    private void UpdateStateText()
    {
        stateText.text = isOn ? "on" : "off";
    }
    
    private void ApplyMaterials()
    {
        // Apply material to background
        if (backgroundImage != null && backgroundOnMaterial != null && backgroundOffMaterial != null)
        {
            // At the endpoints, use the actual material objects
            if (lerpProgress >= 0.99f)
            {
                backgroundImage.material = backgroundOnMaterial;
            }
            else if (lerpProgress <= 0.01f)
            {
                backgroundImage.material = backgroundOffMaterial;
            }
            else
            {
                // In between, use Material.Lerp for gradient transition
                backgroundLerpMaterial.Lerp(backgroundOffMaterial, backgroundOnMaterial, lerpProgress);
                backgroundImage.material = backgroundLerpMaterial;
            }
        }
        
        // Apply material to handle
        if (handleImage != null && handleOnMaterial != null && handleOffMaterial != null)
        {
            // At the endpoints, use the actual material objects
            if (lerpProgress >= 0.99f)
            {
                handleImage.material = handleOnMaterial;
            }
            else if (lerpProgress <= 0.01f)
            {
                handleImage.material = handleOffMaterial;
            }
            else
            {
                // In between, use Material.Lerp for gradient transition
                handleLerpMaterial.Lerp(handleOffMaterial, handleOnMaterial, lerpProgress);
                handleImage.material = handleLerpMaterial;
            }
        }
        
        // Apply material to text
        if (stateText != null && textOnMaterial != null && textOffMaterial != null)
        {
            // At the endpoints, use the actual material objects
            if (lerpProgress >= 0.99f)
            {
                stateText.fontSharedMaterial = textOnMaterial;
            }
            else if (lerpProgress <= 0.01f)
            {
                stateText.fontSharedMaterial = textOffMaterial;
            }
            else
            {
                // In between, use Material.Lerp for gradient transition
                textLerpMaterial.Lerp(textOffMaterial, textOnMaterial, lerpProgress);
                stateText.fontSharedMaterial = textLerpMaterial;
            }
        }
    }

}
