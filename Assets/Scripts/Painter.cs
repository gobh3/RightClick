using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
public class Painter : MonoBehaviour, ITimerClient
{
    private static Painter instance;

    [NonSerialized]
    public UnityEvent<ColorSet> OnPaint;
    
    public float LERP_DURATION = 3.5f;
    public Material ShinyMaterial;
    public Material ShinyTransparentMaterial;
    public Material DarkMaterial;
    public Material NonoShineFont;
    public Material NonoDarkFont;
    public Material  ShinyIconMaterial;
    public UIDocument uiDoc;

    public Camera  MainCamera;
    private LerpRecord<ColorSet> lerpRecord = new LerpRecord<ColorSet>();

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        var root = uiDoc.rootVisualElement;
        //root.style.SetProperty("--main-color", new StyleColor(Color.green));
    }

    public static Painter GetInstance() { return instance; }

    public ColorSet GetCurrentColorSet()
    {
        return lerpRecord.Current;
    }

    public void OnNewLevel(LevelData level)
    {
        ChangeColorSmoothly(level.colorSet);
    }

    public void ChangeColorSmoothly(ColorSet to)
    {
        lerpRecord.To = to;
        AlarmClock.GetInstance().RegisterAndReplace(LERP_DURATION, this);
    }

    public void RegisterToOnColorChanged(UnityAction<ColorSet> updateColor)
    {
        lerpRecord.OnCurrentChanged.AddListener(updateColor);
    }


    public void DuringTimer(float timeElapsed)
    {
        lerpRecord.UpdateCurrent(timeElapsed / LERP_DURATION, ColorFactory.Lerp);
        Color shine = lerpRecord.Current.GetShade(ColorShade.Shine).Value;
        Color dark = lerpRecord.Current.GetShade(ColorShade.Dark).Value;
        ShinyMaterial.color = shine;
        DarkMaterial.color = dark;
        ShinyTransparentMaterial.color = ColorUtil.GetColorWithOpacity(shine, ShinyTransparentMaterial.color.a);
        NonoShineFont.SetColor("_FaceColor", shine);
        NonoDarkFont.SetColor("_FaceColor", dark);
        ShinyIconMaterial.SetColor("_Color", shine);
        MainCamera.backgroundColor = dark;
    }
}

    