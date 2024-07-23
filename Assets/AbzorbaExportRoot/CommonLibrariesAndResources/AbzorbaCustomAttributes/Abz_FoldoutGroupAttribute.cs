namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> As <see cref="Abz_BoxGroupAttribute"></see>, but mimics a foldout section/> </summary>
    public class Abz_FoldoutGroupAttribute : Abz_BoxGroupAttribute
    {
        public Abz_FoldoutGroupAttribute() : base("Foldout Group")
        {
            
        }
        public Abz_FoldoutGroupAttribute(string groupName) : base(groupName)
        {
        }
        public Abz_FoldoutGroupAttribute(string groupName, Abz_GroupColor groupColor) : base(groupName, groupColor)
        {
        }
    }
}