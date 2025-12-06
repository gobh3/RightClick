using UnityEngine;

public class ColorSet
{
    private Color dark;
    private Color shiny;

    //public Color Dark { get => dark; set => dark = value; }
    //public Color Shiny { get => shiny; set => shiny = value; }

    public Color? GetShade(ColorShade shade)
    {
        switch (shade)
        {
            case ColorShade.Dark:
                return dark;
            case ColorShade.Shine:
                return shiny;
        }
        return null;
    }

    public void SetShade(ColorShade shade, Color color)
    {
        switch (shade)
        {
            case ColorShade.Dark:
                dark = color;
                break;
            case ColorShade.Shine:
                shiny = color;
                break;
        }
    }

    override
    public string ToString()
    {
        return dark.ToString();
    }
}