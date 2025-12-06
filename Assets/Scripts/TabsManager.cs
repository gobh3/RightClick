/// TabManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabManager : MonoBehaviour
{
    public List<TabData> tabs;
    public RectTransform underline;
    public float animationDuration = 0.25f;
    public float activeFontSize = 32f;
    public float inactiveFontSize = 24f;
    public float activeAlpha = 1f;
    public float inactiveAlpha = 0.6f;
    public float underlineYOffset = -10f;
    public int defaultTabIndex = 0;

    private int activeTabIndex = 0;
    private Coroutine underlineCoroutine;

    void Start()
    {
        // Clamp defaultTabIndex to valid range
        defaultTabIndex = Mathf.Clamp(defaultTabIndex, 0, tabs.Count - 1);
        activeTabIndex = defaultTabIndex;

        for (int i = 0; i < tabs.Count; i++)
        {
            int index = i;
            tabs[i].tabButton.onClick.AddListener(() => SwitchTab(index));
        }
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].tabContent.SetActive(i == defaultTabIndex);
            tabs[i].tabTitle.fontSize = (i == defaultTabIndex) ? activeFontSize : inactiveFontSize;
            var color = tabs[i].tabTitle.color;
            color.a = (i == defaultTabIndex) ? activeAlpha : inactiveAlpha;
            tabs[i].tabTitle.color = color;
        }
        StartCoroutine(InitUnderlineAfterLayout());
    }

    private IEnumerator InitUnderlineAfterLayout()
    {
        yield return new WaitForEndOfFrame();
        MoveUnderlineInstant(tabs[defaultTabIndex].tabButton.GetComponent<RectTransform>());
    }

    public void SwitchTab(int newIndex)
    {
        if (newIndex == activeTabIndex) return;
        SetActiveTab(newIndex, false);
    }

    private void SetActiveTab(int newIndex, bool instant)
    {
        tabs[activeTabIndex].tabContent.SetActive(false);
        tabs[newIndex].tabContent.SetActive(true);

        StartCoroutine(AnimateTabTitle(tabs[activeTabIndex].tabTitle, false));
        StartCoroutine(AnimateTabTitle(tabs[newIndex].tabTitle, true));

        if (underlineCoroutine != null) StopCoroutine(underlineCoroutine);
        if (instant)
            MoveUnderlineInstant(tabs[newIndex].tabButton.GetComponent<RectTransform>());
        else
            underlineCoroutine = StartCoroutine(MoveUnderline(tabs[newIndex].tabButton.GetComponent<RectTransform>()));

        activeTabIndex = newIndex;
    }

    private IEnumerator AnimateTabTitle(TextMeshProUGUI title, bool active)
    {
        float startSize = title.fontSize;
        float endSize = active ? activeFontSize : inactiveFontSize;
        float startAlpha = title.color.a;
        float endAlpha = active ? activeAlpha : inactiveAlpha;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            title.fontSize = Mathf.Lerp(startSize, endSize, t);
            var color = title.color;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            title.color = color;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        title.fontSize = endSize;
        var finalColor = title.color;
        finalColor.a = endAlpha;
        title.color = finalColor;
    }

    private IEnumerator MoveUnderline(RectTransform target)
    {
        Vector2 startPos = underline.anchoredPosition;
        Vector2 endPos = GetUnderlineTargetPosition(target);
        float startWidth = underline.sizeDelta.x;
        float endWidth = target.sizeDelta.x * 0.5f;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            underline.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            underline.sizeDelta = new Vector2(Mathf.Lerp(startWidth, endWidth, t), underline.sizeDelta.y);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        underline.anchoredPosition = endPos;
        underline.sizeDelta = new Vector2(endWidth, underline.sizeDelta.y);
    }

    private void MoveUnderlineInstant(RectTransform target)
    {
        underline.anchoredPosition = GetUnderlineTargetPosition(target);
        underline.sizeDelta = new Vector2(target.sizeDelta.x * 0.5f, underline.sizeDelta.y);
    }

    private Vector2 GetUnderlineTargetPosition(RectTransform target)
    {
        // Center the underline under the tab, with the specified Y offset
        return new Vector2(target.anchoredPosition.x, target.anchoredPosition.y + underlineYOffset);
    }




    [System.Serializable]
    public class TabData
    {
        public Button tabButton;
        public TextMeshProUGUI tabTitle;
        public GameObject tabContent;
    }
}