namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Renders a Title section above the point it is attributed to.</summary>
    public class Abz_TitleAttribute : Abz_CustomAttribute
    {
        public readonly string Title;
        
        public Abz_TitleAttribute(string title)
        {
            Title = title;
        }        
    }
}
