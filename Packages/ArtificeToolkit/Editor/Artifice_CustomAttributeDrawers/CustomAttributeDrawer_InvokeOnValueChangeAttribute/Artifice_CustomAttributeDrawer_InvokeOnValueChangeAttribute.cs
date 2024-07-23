using CustomAttributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_InvokeOnValueChangeAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(InvokeOnValueChangeAttribute))]
    public class Artifice_CustomAttributeDrawer_InvokeOnValueChangeAttribute : Artifice_CustomAttributeDrawer
    {
        public override void OnPropertyBoundGUI(SerializedProperty property, VisualElement propertyField)
        {
            var attribute = (InvokeOnValueChangeAttribute)Attribute;
            
            var tracker = new VisualElement();
            propertyField.Add(tracker);
            
            // Find method
            // Get reference to target object.
            // If property does not have a SerializedProperty parent, its parent is the serializedObject
            var propertyParent = property.FindParentProperty();
            var parentTarget = propertyParent != null ? propertyParent.GetTarget<object>() : property.serializedObject.targetObject;
            var methodInfo = parentTarget.GetType().GetMethod(attribute.MethodName);
            
            // Subscribe to track
            tracker.TrackPropertyValue(property, changed =>
            {
                methodInfo.Invoke(parentTarget, null);
            });
        }
    }
}
