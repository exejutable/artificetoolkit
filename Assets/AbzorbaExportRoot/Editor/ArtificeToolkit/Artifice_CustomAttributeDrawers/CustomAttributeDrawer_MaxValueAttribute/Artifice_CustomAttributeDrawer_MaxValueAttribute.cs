using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_MaxValueAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(Abz_MaxValueAttribute))]
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
            var attribute = (Abz_MaxValueAttribute)Attribute;

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
