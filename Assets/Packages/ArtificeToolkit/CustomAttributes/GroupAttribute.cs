namespace Packages.ArtificeToolkit.CustomAttributes
{
    public enum Abz_GroupColor
    {
        Default,
        Red,
        Blue,
        Black,
        Green,
        Yellow,
        Orange,
        Pink,
        Purple
    }
    
    /// <summary> All Group-like attributes should inherit from this class. </summary>
    public abstract class GroupAttribute : CustomAttribute
    {
        public readonly string GroupName;
        public readonly Abz_GroupColor GroupColor;

        protected GroupAttribute(string groupName)
        {
            GroupName = groupName;
            GroupColor = Abz_GroupColor.Default;
        }
        protected GroupAttribute(string groupName, Abz_GroupColor groupColor)
        {
            GroupName = groupName;
            GroupColor = groupColor;
        }
    }
}
