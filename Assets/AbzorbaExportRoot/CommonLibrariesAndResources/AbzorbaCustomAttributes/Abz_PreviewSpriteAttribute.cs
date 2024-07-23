using UnityEngine;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Allows a <see cref="Sprite"/> or <see cref="Texture2D"/> to be shown as an image rather than a text-based object field. </summary>
    public class Abz_PreviewSpriteAttribute : Abz_CustomAttribute
    {
        public readonly int Width = 75;
        public readonly int Height = 75;
        
        public Abz_PreviewSpriteAttribute()
        {
            
        }
        public Abz_PreviewSpriteAttribute(int dimensions = 75, bool shouldRenderInline = false)
        {
            Width = Height = dimensions;
        }
        public Abz_PreviewSpriteAttribute(int width, int height, bool shouldRenderInline = false)
        {
            Width = width;
            Height = height;
        }
    }
}
