using ArtificeToolkit.Attributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_BorderAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(BorderAttribute))]
    public class Artifice_CustomAttributeDrawer_BorderAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnWrapGUI(SerializedProperty property, VisualElement root)
        {
            var attribute = (BorderAttribute)Attribute;
            
            root.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            root.AddToClassList("border");
            root.style.borderTopColor = attribute.Color;
            root.style.borderBottomColor = attribute.Color;
            root.style.borderLeftColor = attribute.Color;
            root.style.borderRightColor = attribute.Color;

            return root;
        }
    }
}
