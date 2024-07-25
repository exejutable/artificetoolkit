namespace ArtificeToolkit.Attributes
{
    /// <summary> As <see cref="BoxGroupAttribute"></see>, but mimics a foldout section/> </summary>
    public class FoldoutGroupAttribute : BoxGroupAttribute
    {
        public FoldoutGroupAttribute() : base("Foldout Group")
        {
            
        }
        public FoldoutGroupAttribute(string groupName) : base(groupName)
        {
        }
        public FoldoutGroupAttribute(string groupName, GroupColor groupColor) : base(groupName, groupColor)
        {
        }
    }
}