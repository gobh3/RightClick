using System;
using UnityEngine;

public class ColorFactory
{
    private const float S = 100f/100f;
    private const float A = 100f/100f;
    private const float ShinyV = 100f/100f;
    private const float DarkV = 17f/100f;
    
    public static ColorSet GetRandomColorAPI()
    {
        float randomH = UnityEngine.Random.Range(0, 361)/360f;
        Color randomDark = Color.HSVToRGB(randomH, S, DarkV);
        randomDark.a = A;
        Color randomShiny = Color.HSVToRGB(randomH, ShinyV, ShinyV);
        randomShiny.a = A;
        ColorSet colorSet = new ColorSet();
        colorSet.SetShade(ColorShade.Shine, randomShiny);
        colorSet.SetShade(ColorShade.Dark, randomDark);
        return colorSet;
    }

    public static ColorSet Lerp(ColorSet colorFrom, ColorSet colorTo, float fraction)
    {
        ColorSet colorSet = new ColorSet();
        Color darkColor = Color.Lerp(colorFrom.GetShade(ColorShade.Dark).Value, colorTo.GetShade(ColorShade.Dark).Value, fraction);
        Color shinnyColor = Color.Lerp(colorFrom.GetShade(ColorShade.Shine).Value, colorTo.GetShade(ColorShade.Shine).Value, fraction);
        colorSet.SetShade(ColorShade.Dark, darkColor);
        colorSet.SetShade(ColorShade.Shine, shinnyColor);
        return colorSet;
    }

}