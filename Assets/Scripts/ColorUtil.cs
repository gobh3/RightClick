using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtil
{
    public static Color GetColorWithOpacity(Color c, float a)
    {
        c.a = a;
        return c;
    }
}
