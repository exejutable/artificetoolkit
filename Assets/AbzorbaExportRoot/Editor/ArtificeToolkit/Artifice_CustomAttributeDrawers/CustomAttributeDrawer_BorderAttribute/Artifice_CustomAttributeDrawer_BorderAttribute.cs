using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_BorderAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(Abz_BorderAttribute))]
    public class Artifice_CustomAttributeDrawer_BorderAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnWrapGUI(SerializedProperty property, VisualElement root)
        {
            var attribute = (Abz_BorderAttribute)Attribute;
            
            root.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            root.AddToClassList("abz-border");
            root.style.borderTopColor = attribute.Color;
            root.style.borderBottomColor = attribute.Color;
            root.style.borderLeftColor = attribute.Color;
            root.style.borderRightColor = attribute.Color;

            return root;
        }
    }
}
