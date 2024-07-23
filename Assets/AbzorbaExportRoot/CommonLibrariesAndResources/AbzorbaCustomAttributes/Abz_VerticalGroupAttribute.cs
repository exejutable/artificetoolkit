namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> This attribute on its own is not useful, but comes into play in parallel with the <see cref="Abz_HorizontalGroupAttribute"/> </summary>
    public class Abz_VerticalGroupAttribute : Abz_GroupAttribute
    {
        public Abz_VerticalGroupAttribute() : base("Vertical Group")
        {
            
        }
        public Abz_VerticalGroupAttribute(string groupName) : base(groupName)
        {
        }
    }
}
