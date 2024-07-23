using AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CommonResources;
using AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_VisualElements.AbzEditor_VisualElement_InfoBox;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_Validators
{
    public abstract class Artifice_CustomAttributeDrawer_Validator_BASE : Artifice_CustomAttributeDrawer
    {
        #region FIELDS

        protected VisualElement ValidatorLogElem;

        public abstract string LogMessage { get; }
        public abstract Sprite LogSprite { get; }
        public abstract LogType LogType { get; }

        #endregion
        
        public override VisualElement OnPrePropertyGUI(SerializedProperty property)
        {
            ValidatorLogElem = new VisualElement();
            
            // Check if this is a valid attribute
            if (!IsApplicableToProperty(property))
                return new Artifice_VisualElement_InfoBox($"Attribute is not applicable for this property [{property.name}]", Artifice_SCR_CommonResourcesHolder.instance.ErrorIcon);

            // Add error on container and hide unless stated otherwise
            ValidatorLogElem.Add(new Artifice_VisualElement_InfoBox(LogMessage, LogSprite));
            
            // Fire once for existing value
            OnPropertyValueChanged(property);
            // Track changes
            ValidatorLogElem.TrackPropertyValue(property, OnPropertyValueChanged);
            
            return ValidatorLogElem;
        }

        /// <summary> Check whether the <see cref="SerializedProperty"/>.propertyType is inline with the attribute's logic. </summary>
        protected abstract bool IsApplicableToProperty(SerializedProperty property);
        
        /// <summary> Custom logic to check whether the <see cref="SerializedProperty"/> is valid based on the validator </summary>
        public abstract bool IsValid(SerializedProperty property);

        protected virtual void OnPropertyValueChanged(SerializedProperty property)
        {
            if(IsValid(property))
                ValidatorLogElem.AddToClassList("hide");
            else
                ValidatorLogElem.RemoveFromClassList("hide");
        }
    }
}
