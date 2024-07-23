using UnityEngine;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Wraps property in a colored border. </summary>
    public class Abz_BorderAttribute : Abz_CustomAttribute
    {
        public readonly Color Color;

        public Abz_BorderAttribute()
        {
            Color = Color.black;
        }

        public Abz_BorderAttribute(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color parsedColor);
            Color = parsedColor;
        }
        
        public Abz_BorderAttribute(float r, float g, float b, float a = 1)
        {
            Color = new Color(r, g, b, a);
        }
    }
}