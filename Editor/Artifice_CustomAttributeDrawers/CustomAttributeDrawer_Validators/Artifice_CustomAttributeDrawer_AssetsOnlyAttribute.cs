using ArtificeToolkit.Attributes;
using ArtificeToolkit.AEditor.Resources;
using UnityEditor;
using UnityEngine;

namespace ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_Validators
{
    [Artifice_CustomAttributeDrawer(typeof(AssetsOnlyAttribute))]
    public class Artifice_CustomAttributeDrawer_AssetsOnlyAttribute : Artifice_CustomAttributeDrawer_Validator_BASE
    {
        public override string LogMessage { get; } = "Property must be an Asset";
        public override Sprite LogSprite { get; } = Artifice_SCR_CommonResourcesHolder.instance.ErrorIcon;
        public override LogType LogType { get; } = LogType.Error; 

        protected override bool IsApplicableToProperty(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }

        public override bool IsValid(SerializedProperty property)
        {
            return property.objectReferenceValue == null || AssetDatabase.Contains(property.objectReferenceValue);
        }
    }
}