namespace ArtificeToolkit.Attributes
{
    /// <summary> Wraps the element inside a titled box. It can also be used with nesting. </summary>
    /// <example> [BoxGroupAttribute("Outer Box/Interior Box")]</example>
    public class BoxGroupAttribute : GroupAttribute
    {
        public BoxGroupAttribute() : base("Box Group")
        {
        }
        public BoxGroupAttribute(string groupName) : base(groupName)
        {
        }
        public BoxGroupAttribute(string groupName, GroupColor groupColor) : base(groupName, groupColor)
        {
        }
    }
}