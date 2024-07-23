namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    public class Abz_TabGroupAttribute : Abz_GroupAttribute
    {
        public readonly string TabSection;
        
        public Abz_TabGroupAttribute(string tabSection) : base("Default Tab Group")
        {
            TabSection = tabSection;
        }
        
        public Abz_TabGroupAttribute(string tabSection, Abz_GroupColor groupColor) : base("Default Tab Group", groupColor)
        {
            TabSection = tabSection;
        }
        
        public Abz_TabGroupAttribute(string groupName, string tabSection) : base(groupName)
        {
            TabSection = tabSection;
        }
        
        public Abz_TabGroupAttribute(string groupName, string tabSection, Abz_GroupColor groupColor) : base(groupName, groupColor)
        {
            TabSection = tabSection;
        }
    }
}