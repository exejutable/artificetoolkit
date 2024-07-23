using Packages.ArtificeToolkit.CustomAttributes;
using Packages.ArtificeToolkit.Editor.Artifice_CommonResources;
using UnityEditor;
using UnityEngine;

namespace Packages.ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_Validators
{
    [Artifice_CustomAttributeDrawer(typeof(SceneObjectsOnlyAttribute))]
    public class Artifice_CustomAttributeDrawer_SceneObjectsOnlyAttribute : Artifice_CustomAttributeDrawer_Validator_BASE
    {
        public override string LogMessage { get; } = "Property must be a scene object";
        public override Sprite LogSprite { get; } = Artifice_SCR_CommonResourcesHolder.instance.ErrorIcon;
        public override LogType LogType { get; } = LogType.Error;

        protected override bool IsApplicableToProperty(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }

        public override bool IsValid(SerializedProperty property)
        {
            return property.objectReferenceValue == null || AssetDatabase.Contains(property.objectReferenceValue) == false;
        }
    }
}