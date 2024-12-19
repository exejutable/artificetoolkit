using ArtificeToolkit.Attributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_ReadOnlyAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(ReadOnlyAttribute))]
    public class Artifice_CustomAttributeDrawer_ReadOnlyAttribute : Artifice_CustomAttributeDrawer
    {
        // Add a semi transparent blocker and disable interactability.
        public override void OnPropertyBoundGUI(SerializedProperty property, VisualElement propertyField)
        {
            // Get style
            propertyField.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            
            // Field generic base input field and draw a semi transparent rect above it.
            var inputFields = propertyField.Query(className: "unity-property-field__input").ToList();
            foreach(var inputField in inputFields)
            {
                var transparentBlocker = new VisualElement();
                transparentBlocker.AddToClassList("blocker");
                inputField.Add(transparentBlocker);
                
                transparentBlocker.tooltip = "This property is marked as Read-Only.\n";
            }

            // Make all input field non interactable.
            DisableInteractivity(propertyField);
        }
        
        // Recursively disable interactivity for all elements
        private void DisableInteractivity(VisualElement element)
        {
            // Set the picking mode to Ignore to make elements non-interactable
            element.pickingMode = PickingMode.Ignore;

            // If there are child elements, recursively disable them as well
            foreach (var child in element.Children())
                DisableInteractivity(child);
        }
    }
}
