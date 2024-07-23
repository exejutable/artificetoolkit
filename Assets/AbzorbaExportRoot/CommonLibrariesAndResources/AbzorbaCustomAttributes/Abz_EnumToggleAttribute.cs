namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> This attribute changes the way default Enums are rendered to have them as toggle buttons </summary>
    /// <remarks> Automatically detects and handles enums which are marked as Flagged. </remarks>
    public class Abz_EnumToggleAttribute : Abz_CustomAttribute
    {
        public readonly bool HideLabel = false;
        
        public Abz_EnumToggleAttribute(bool hideLabel = false) : base()
        {
            HideLabel = hideLabel;
        }
    }
}
