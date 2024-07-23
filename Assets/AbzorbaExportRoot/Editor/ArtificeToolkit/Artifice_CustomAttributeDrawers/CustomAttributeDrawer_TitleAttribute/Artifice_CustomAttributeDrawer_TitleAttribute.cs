using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_TitleAttribute
{
    /// <summary> Custom VisualAttribute drawer for <see cref="Abz_TitleAttribute"/></summary>
    [Artifice_CustomAttributeDrawer(typeof(Abz_TitleAttribute))]
    public class Artifice_CustomAttributeDrawer_TitleAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnPrePropertyGUI(SerializedProperty property)
        {
            var attribute = (Abz_TitleAttribute)Attribute;
            
            var titleLabel = new Label(attribute.Title);
            titleLabel.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            titleLabel.AddToClassList("title");

            return titleLabel;
        }
    }
}