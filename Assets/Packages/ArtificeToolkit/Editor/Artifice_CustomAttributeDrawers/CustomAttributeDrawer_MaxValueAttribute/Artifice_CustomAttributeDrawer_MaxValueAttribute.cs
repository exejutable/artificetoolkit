using System;
using Packages.ArtificeToolkit.CustomAttributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Packages.ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_MaxValueAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(MaxValueAttribute))]
    public class Artifice_CustomAttributeDrawer_MaxValueAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnPrePropertyGUI(SerializedProperty property)
        {
            var tracker = new VisualElement();
            tracker.TrackPropertyValue(property, OnValueChanged);
            return tracker;
        }

        private void OnValueChanged(SerializedProperty property)
        {
            var attribute = (MaxValueAttribute)Attribute;

            property.serializedObject.Update();
            
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = Mathf.Min(property.intValue, (int)attribute.Value);
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = Mathf.Min(property.floatValue, attribute.Value);
                    break;
                default:
                    throw new ArgumentException($"MaxValueAttribute not supported for type {property.propertyType}");
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
