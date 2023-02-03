using System;
using UnityEngine;

namespace Game
{
    public static class ColorExtensions
    {
        public static Color Blend(this Color color1,Color color2,float blendingValue)
        {
            blendingValue = Mathf.Clamp01(blendingValue);
            
            float r = (color1.r*(1-blendingValue))+(color2.r*blendingValue);
            float g = (color1.g*(1-blendingValue))+(color2.g*blendingValue);
            float b = (color1.b*(1-blendingValue))+(color2.b*blendingValue);
            
            return new Color(r,g,b);
        }
    }
}