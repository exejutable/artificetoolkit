using System.Diagnostics;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
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
    public abstract class Abz_GroupAttribute : Abz_CustomAttribute
    {
        public readonly string GroupName;
        public readonly Abz_GroupColor GroupColor;

        protected Abz_GroupAttribute(string groupName)
        {
            GroupName = groupName;
            GroupColor = Abz_GroupColor.Default;
        }
        protected Abz_GroupAttribute(string groupName, Abz_GroupColor groupColor)
        {
            GroupName = groupName;
            GroupColor = groupColor;
        }
    }
}
