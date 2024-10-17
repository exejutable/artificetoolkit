using ArtificeToolkit.Attributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_HideInArtificeAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(HideInArtificeAttribute))]
    public class Artifice_CustomAttributeDrawer_HideInArtificeAttribute : Artifice_CustomAttributeDrawer
    {
        public override void OnPropertyBoundGUI(SerializedProperty property, VisualElement propertyField)
        {
            propertyField.AddToClassList("hide");
        }
        
    }
}