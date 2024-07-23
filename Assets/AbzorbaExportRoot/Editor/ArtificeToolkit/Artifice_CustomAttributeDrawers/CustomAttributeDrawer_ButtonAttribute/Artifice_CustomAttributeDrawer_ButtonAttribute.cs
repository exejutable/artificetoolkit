using System;
using System.Collections.Generic;
using System.Text;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_VisualElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_ButtonAttribute
{
    [Serializable]
    [Artifice_CustomAttributeDrawer(typeof(Abz_ButtonAttribute))]
    public class Artifice_CustomAttributeDrawer_ButtonAttribute : Artifice_CustomAttributeDrawer
    {
        public override bool IsReplacingPropertyField { get; } = true;

        public override VisualElement OnPropertyGUI(SerializedProperty property)
        {
            var attribute = (Abz_ButtonAttribute)Attribute;

            // Get reference to target object.
            // If property does not have a SerializedProperty parent, its parent is the serializedObject
            var propertyParent = property.FindParentProperty();
            var parentTarget = propertyParent != null ? propertyParent.GetTarget<object>() : property.serializedObject.targetObject;
            var methodInfo = parentTarget.GetType().GetMethod(attribute.MethodName);

            // Error check the method
            if (methodInfo == null)
            {
                Debug.LogError($"Unable to find method {attribute.MethodName} in {property.serializedObject.targetObject.name}");
                return new VisualElement();
            }
            
            // Create main container
            var container = new VisualElement();
            container.AddToClassList("button-container");
            
            // Create button to invoke and return!
            var button = new Artifice_VisualElement_LabeledButton(AddSpacesBeforeCapitals(attribute.MethodName), () =>
            {
                property.serializedObject.Update();
                
                // Create parameters array
                var parametersList = GetParameterList(property);
                // Error check number of parameters
                if (methodInfo.GetParameters().Length != parametersList.Count)
                    throw new ArgumentException($"[Abz_Button] Parameters count do not match with method {methodInfo.Name}");
                
                // Call method with params
                methodInfo.Invoke(parentTarget, parametersList.ToArray());
                
                property.serializedObject.ApplyModifiedProperties();
            });
            button.style.paddingBottom = 10;
            button.style.paddingTop = 10;
            container.Add(button);

            return container;
        }

        private List<object> GetParameterList(SerializedProperty property)
        {
            var attribute = (Abz_ButtonAttribute)Attribute;
            var parametersList = new List<object>();
            
            // Iterate and find parameters
            foreach (var parameterName in attribute.ParameterNames)
            {
                var parameterProperty = property.FindPropertyInSameScope(parameterName);
                if (parameterProperty == null)
                    throw new ArgumentException($"[Abz_Button] Cannot find parameter name {parameterName}");
                
                parametersList.Add(parameterProperty.GetTarget<object>());
            }

            return parametersList;
        }
        
        private string AddSpacesBeforeCapitals(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var spacedString = new StringBuilder();
            spacedString.Append(input[0]);

            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    spacedString.Append(' '); // Add a space before capital letter
                }
                spacedString.Append(input[i]);
            }

            return spacedString.ToString();
        }
    }
}
