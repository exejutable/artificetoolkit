using System;
using Packages.ArtificeToolkit.CustomAttributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Packages.ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_MinValueAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(MinValueAttribute))]
    public class Artifice_CustomAttributeDrawer_MinValueAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnPrePropertyGUI(SerializedProperty property)
        {
            var tracker = new VisualElement();
            tracker.TrackPropertyValue(property, OnValueChanged);
            return tracker;
        }

        private void OnValueChanged(SerializedProperty property)
        {
            var attribute = (MinValueAttribute)Attribute;
            
            property.serializedObject.Update();
            
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = Mathf.Max(property.intValue, (int)attribute.Value);
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = Mathf.Max(property.floatValue, attribute.Value);
                    break;
                default:
                    throw new ArgumentException($"MinValueAttribute not supported for type {property.propertyType}");
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
