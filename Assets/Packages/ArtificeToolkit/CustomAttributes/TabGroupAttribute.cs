namespace Packages.ArtificeToolkit.CustomAttributes
{
    public class TabGroupAttribute : GroupAttribute
    {
        public readonly string TabSection;
        
        public TabGroupAttribute(string tabSection) : base("Default Tab Group")
        {
            TabSection = tabSection;
        }
        
        public TabGroupAttribute(string tabSection, Abz_GroupColor groupColor) : base("Default Tab Group", groupColor)
        {
            TabSection = tabSection;
        }
        
        public TabGroupAttribute(string groupName, string tabSection) : base(groupName)
        {
            TabSection = tabSection;
        }
        
        public TabGroupAttribute(string groupName, string tabSection, Abz_GroupColor groupColor) : base(groupName, groupColor)
        {
            TabSection = tabSection;
        }
    }
}