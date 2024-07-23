namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Wraps the element inside a titled box. It can also be used with nesting. </summary>
    /// <example> [Abz_BoxGroupAttribute("Outer Box/Interior Box")]</example>
    public class Abz_BoxGroupAttribute : Abz_GroupAttribute
    {
        public Abz_BoxGroupAttribute() : base("Box Group")
        {
        }
        public Abz_BoxGroupAttribute(string groupName) : base(groupName)
        {
        }
        public Abz_BoxGroupAttribute(string groupName, Abz_GroupColor groupColor) : base(groupName, groupColor)
        {
        }
    }
}