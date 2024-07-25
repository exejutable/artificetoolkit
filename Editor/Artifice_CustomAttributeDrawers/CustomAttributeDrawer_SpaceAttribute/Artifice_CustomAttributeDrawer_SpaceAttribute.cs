using ArtificeToolkit.Attributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_SpaceAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(SpaceAttribute))]
    public class Artifice_CustomAttributeDrawer_SpaceAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnWrapGUI(SerializedProperty property, VisualElement root)
        {
            var attribute = (SpaceAttribute)Attribute;
            var container = new VisualElement();
            container.Add(root);
            container.style.marginTop = attribute.ValueTop;
            container.style.marginBottom = attribute.ValueBottom;
            container.style.marginLeft = attribute.ValueLeft;
            container.style.marginRight = attribute.ValueRight;
            
            return container;
        }
    }
}
