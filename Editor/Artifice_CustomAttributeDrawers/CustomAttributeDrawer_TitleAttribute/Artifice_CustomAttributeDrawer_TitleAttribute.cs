using ArtificeToolkit.Attributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_TitleAttribute
{
    /// <summary> Custom VisualAttribute drawer for <see cref="TitleAttribute"/></summary>
    [Artifice_CustomAttributeDrawer(typeof(TitleAttribute))]
    public class Artifice_CustomAttributeDrawer_TitleAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnPrePropertyGUI(SerializedProperty property)
        {
            var attribute = (TitleAttribute)Attribute;
            
            var titleLabel = new Label(attribute.Title);
            titleLabel.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            titleLabel.AddToClassList("title");

            return titleLabel;
        }
    }
}