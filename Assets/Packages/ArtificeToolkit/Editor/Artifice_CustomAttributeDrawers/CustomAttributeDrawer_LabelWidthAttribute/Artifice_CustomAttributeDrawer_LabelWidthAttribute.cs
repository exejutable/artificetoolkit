using Packages.ArtificeToolkit.CustomAttributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace Packages.ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_LabelWidthAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(LabelWidthAttribute))]
    public class Artifice_CustomAttributeDrawer_LabelWidthAttribute : Artifice_CustomAttributeDrawer
    {
        public override void OnPropertyBoundGUI(SerializedProperty property, VisualElement propertyField)
        {
            // If control uses PropertyField, it will construct after bind which happens before attaching to panel.
            // So wait for the event until the logic is called!
            var attribute = (LabelWidthAttribute)Attribute;       
            var labelFields = propertyField.Query<Label>().ToList();
            
            foreach (var label in labelFields)
            {
                label.style.maxWidth = attribute.Width;
                label.style.minWidth = attribute.Width;
            }            
        }
    }
}
