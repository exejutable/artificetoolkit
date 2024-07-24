using UnityEngine;

namespace CustomAttributes
{
    /// <summary> Wraps property in a colored border. </summary>
    public class BorderAttribute : CustomAttribute
    {
        public readonly Color Color;

        public BorderAttribute()
        {
            Color = Color.black;
        }

        public BorderAttribute(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color parsedColor);
            Color = parsedColor;
        }
        
        public BorderAttribute(float r, float g, float b, float a = 1)
        {
            Color = new Color(r, g, b, a);
        }
    }
}