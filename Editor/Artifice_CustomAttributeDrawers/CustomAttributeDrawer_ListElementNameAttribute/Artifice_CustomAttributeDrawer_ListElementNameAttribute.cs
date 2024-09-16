using ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers;
using CustomAttributes;

namespace Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_ListElementNameAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(ListElementNameAttribute))]
    public class Artifice_CustomAttributeDrawer_ListElementNameAttribute : Artifice_CustomAttributeDrawer
    {   
        /*
         * No implementation here. Drawer added just for consistency and to follow pattern.
         *
         * This element will be applied and checked internally through the Artifice_VisualElement_AbstractListView
         */
    }
}
