using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GenericToggleController : MonoBehaviour
{
    [Header("Toggle Settings")]
    [Tooltip("The name of the toggle in the UXML file.")]
    public string toggleName;

    [Header("Toggle Events")]
    public UnityEvent onToggleOn;
    public UnityEvent onToggleOff;
    public UnityEvent<GenericToggleController> onToggleEnabled;
    private Toggle targetToggle;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        if (string.IsNullOrEmpty(toggleName))
        {
            Debug.LogError("Toggle name not set in " + gameObject.name);
            return;
        }

        targetToggle = root.Q<Toggle>(toggleName);

        if (targetToggle == null)
        {
            Debug.LogError($"No toggle found with name '{toggleName}' in {gameObject.name}");
            return;
        }

        targetToggle.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue)
            {
                onToggleOn?.Invoke();
            }
            else
            {
                onToggleOff?.Invoke();
            }
        });

        onToggleEnabled?.Invoke(this);

    }

    public void SetValue(bool setActive)
    {
        targetToggle.value = setActive;
    }
}
