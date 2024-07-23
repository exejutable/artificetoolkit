using UnityEngine;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.ArtificeAttributes
{
    /// <summary> Allows a <see cref="Sprite"/> or <see cref="Texture2D"/> to be shown as an image rather than a text-based object field. </summary>
    public class PreviewSpriteAttribute : CustomAttribute
    {
        public readonly int Width = 75;
        public readonly int Height = 75;
        
        public PreviewSpriteAttribute()
        {
            
        }
        public PreviewSpriteAttribute(int dimensions = 75, bool shouldRenderInline = false)
        {
            Width = Height = dimensions;
        }
        public PreviewSpriteAttribute(int width, int height, bool shouldRenderInline = false)
        {
            Width = width;
            Height = height;
        }
    }
}
