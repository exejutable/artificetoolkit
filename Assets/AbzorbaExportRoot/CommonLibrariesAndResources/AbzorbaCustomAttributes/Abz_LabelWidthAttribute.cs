namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Controls the width of Label Fields </summary>
    public class Abz_LabelWidthAttribute : Abz_CustomAttribute
    {
        public readonly int Width;

        private Abz_LabelWidthAttribute()
        {
            
        }

        public Abz_LabelWidthAttribute(int width) : this()
        {
            Width = width;
        }
    }
}
