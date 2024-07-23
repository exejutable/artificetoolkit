using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.ArtificeAttributes;
using AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CommonResources;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_Validators
{
    [Artifice_CustomAttributeDrawer(typeof(ChildGameObjectsOnlyAttribute))]
    public class Artifice_CustomAttributeDrawer_ChildGameObjectsOnlyAttribute : Artifice_CustomAttributeDrawer_Validator_BASE
    {
        #region FIELDS

        private bool _disposed;
        private Artifice_EditorWindow_GameObjectBrowser _browserWin;
        
        public override string LogMessage { get; } = "Property must be an Object which is a child of this Object.";
        public override Sprite LogSprite { get; } = Artifice_SCR_CommonResourcesHolder.instance.ErrorIcon;
        public override LogType LogType { get; } = LogType.Error;

        #endregion
        
        public override VisualElement OnWrapGUI(SerializedProperty property, VisualElement root)
        {
            var wrapper = new VisualElement();
            wrapper.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            wrapper.AddToClassList("validator-container");
            
            // Add property as is
            root.AddToClassList("property-field");
            wrapper.Add(root);
            
            // Add small icon that will allow  browsing children properties.
            var browserButton = new Image();
            browserButton.sprite = Artifice_SCR_CommonResourcesHolder.instance.MagnifyingGlassIcon;
            browserButton.AddToClassList("browse-button");
            wrapper.Add(browserButton);
            
            // Subscribe to button click
            browserButton.RegisterCallback<MouseDownEvent>(evt =>
            {
                // Get property type
                var gameObject = ((Behaviour)property.serializedObject.targetObject).gameObject;
                var searchType = property.GetTargetType();
                
                // Get mouse position and set up position struct for editor window
                var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                    
                // Open editor window and pass on data
                var browser = ScriptableObject.CreateInstance<Artifice_EditorWindow_GameObjectBrowser>();
                browser.Browse(gameObject, searchType, mousePos);

                // Subscribe to selection event
                browser.OnObjectSelected.AddListener(obj =>
                {
                    property.objectReferenceValue = obj;
                    property.serializedObject.ApplyModifiedProperties();
                    browser.Close();
                });
            });
            
            return wrapper;
        }

        protected override bool IsApplicableToProperty(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }
        public override bool IsValid(SerializedProperty property)
        {
            // If value is null, just return true.
            if (property.objectReferenceValue == null)
                return true;
            
            // Get parent gameobject through serializedObject target
            var script = property.serializedObject.targetObject as Component;
            if (script == null)
                throw new InvalidCastException("ChildGameObjectOnly attribute works for Object scripts only.");

            // Get root gameobject
            var rootGo = script.gameObject;
            
            // Get property value
            // Case: GameObject, check transform
            GameObject propertyValueGo = null;
            if (property.objectReferenceValue is GameObject go)
                propertyValueGo = go;
            else if (property.objectReferenceValue is Component propertyValueScript)
            {
                propertyValueGo = propertyValueScript.gameObject;
            }
            else
                throw new Exception();
            
            // Return result
            return propertyValueGo.transform.IsChildOf(rootGo.transform);
        }
    }
}
