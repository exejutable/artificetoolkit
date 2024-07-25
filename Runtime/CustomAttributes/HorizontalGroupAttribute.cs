namespace ArtificeToolkit.Attributes
{
    /// <summary> As <see cref="BoxGroupAttribute"/>, but uses horizontal alignment. </summary>
    /// <remarks> Current implementation does not cope well with input fields. </remarks>
    public class HorizontalGroupAttribute : BoxGroupAttribute
    {
        public readonly float WidthPercent = -1;
        
        public HorizontalGroupAttribute() : base("Horizontal Group")
        {
            
        }
        public HorizontalGroupAttribute(string groupName) : base(groupName)
        {
            
        }
        public HorizontalGroupAttribute(string groupName, float widthPercentPercent = -1) : this(groupName)
        {
            WidthPercent = widthPercentPercent;
        }
    }
}
