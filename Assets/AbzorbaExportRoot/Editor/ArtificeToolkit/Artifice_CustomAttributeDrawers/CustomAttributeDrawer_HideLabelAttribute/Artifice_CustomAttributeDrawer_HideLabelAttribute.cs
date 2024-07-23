using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_HideLabelAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(Abz_HideLabelAttribute))]
    public class Artifice_CustomAttributeDrawer_HideLabelAttribute : Artifice_CustomAttributeDrawer
    {
        public override void OnPropertyBoundGUI(SerializedProperty property, VisualElement propertyField)
        {
            if (propertyField.childCount > 0)
            {
                var label = propertyField.Query<Label>().First();
                label?.AddToClassList("hide");
            }
            else
            {
                propertyField.RegisterCallback<GeometryChangedEvent>(evt =>
                {
                    var label = propertyField.Query<Label>().First();
                    label?.AddToClassList("hide");
                });
            }
        }
    }
}
