namespace ArtificeToolkit.Attributes
{
    public class TabGroupAttribute : GroupAttribute
    {
        public readonly string TabSection;
        
        public TabGroupAttribute(string tabSection) : base("Default Tab Group")
        {
            TabSection = tabSection;
        }
        
        public TabGroupAttribute(string tabSection, GroupColor groupColor) : base("Default Tab Group", groupColor)
        {
            TabSection = tabSection;
        }
        
        public TabGroupAttribute(string groupName, string tabSection) : base(groupName)
        {
            TabSection = tabSection;
        }
        
        public TabGroupAttribute(string groupName, string tabSection, GroupColor groupColor) : base(groupName, groupColor)
        {
            TabSection = tabSection;
        }
    }
}