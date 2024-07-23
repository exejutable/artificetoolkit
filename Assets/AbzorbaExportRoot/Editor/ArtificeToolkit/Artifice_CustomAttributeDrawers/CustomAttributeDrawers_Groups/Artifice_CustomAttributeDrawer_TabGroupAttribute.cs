using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using UnityEditor;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    [Artifice_CustomAttributeDrawer(typeof(Abz_TabGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_TabGroupAttribute : Artifice_CustomAttributeDrawer_GroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_TabGroup);

        protected override Artifice_VisualElement_Group CreateOrGetContainer(SerializedProperty property)
        {
            var attribute = (Abz_TabGroupAttribute)Attribute;

            var groupTuple = Artifice_CustomAttributeUtility_GroupsHolder.Instance.Get(property, attribute.GroupName, VisualElementType);
            
            var tabGroup = (Artifice_VisualElement_TabGroup)groupTuple.lastElem;
            tabGroup.SetGroupColor(attribute.GroupColor);
            if(tabGroup.ContainsTab(attribute.TabSection) == false)
                tabGroup.CreateTab(attribute.TabSection);
            tabGroup.SelectTab(attribute.TabSection);            
            
            // Reset to first tab always
            tabGroup.LoadPersistedData();
            
            return groupTuple.firstElem;
        }
    }
}