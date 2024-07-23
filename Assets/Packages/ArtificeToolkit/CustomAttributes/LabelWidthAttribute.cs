namespace Packages.ArtificeToolkit.CustomAttributes
{
    /// <summary> Controls the width of Label Fields </summary>
    public class LabelWidthAttribute : CustomAttribute
    {
        public readonly int Width;

        private LabelWidthAttribute()
        {
            
        }

        public LabelWidthAttribute(int width) : this()
        {
            Width = width;
        }
    }
}
