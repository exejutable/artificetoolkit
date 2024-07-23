namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> As <see cref="Abz_BoxGroupAttribute"/>, but uses horizontal alignment. </summary>
    /// <remarks> Current implementation does not cope well with input fields. </remarks>
    public class Abz_HorizontalGroupAttribute : Abz_BoxGroupAttribute
    {
        public readonly float WidthPercent = -1;
        
        public Abz_HorizontalGroupAttribute() : base("Horizontal Group")
        {
            
        }
        public Abz_HorizontalGroupAttribute(string groupName) : base(groupName)
        {
            
        }
        public Abz_HorizontalGroupAttribute(string groupName, float widthPercentPercent = -1) : this(groupName)
        {
            WidthPercent = widthPercentPercent;
        }
    }
}
