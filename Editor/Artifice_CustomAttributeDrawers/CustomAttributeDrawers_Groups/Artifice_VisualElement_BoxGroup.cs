using ArtificeToolkit.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Utility VisualElement used for <see cref="Artifice_CustomAttributeDrawer_BoxGroupAttribute"/> </summary>
    /// <remarks> Offers API for setting and reseting its content container. </remarks>
    public class Artifice_VisualElement_BoxGroup : Artifice_VisualElement_Group
    {
        private readonly VisualElement _titleContainer;
        private readonly Label _titleLabel;

        protected Artifice_VisualElement_BoxGroup() : base()
        {
            styleSheets.Add(Artifice_Utilities.GetStyle(typeof(Artifice_VisualElement_Group)));
            styleSheets.Add(Artifice_Utilities.GetStyle(typeof(Artifice_VisualElement_BoxGroup)));
            
            // Create title container and its label
            _titleContainer = new VisualElement();
            _titleContainer.AddToClassList("align-horizontal");
            _titleContainer.AddToClassList("title-container");
            hierarchy.Insert(0, _titleContainer);
                
            _titleLabel = new Label("Undefined");
            _titleLabel.AddToClassList("title-label");
            _titleContainer.Add(_titleLabel);
        }

        public override void SetTitle(string title)
        {
            _titleLabel.text = title;
        }

        public override void SetGroupColor(GroupColor groupColor)
        {
            base.SetGroupColor(groupColor);
            _titleContainer.style.backgroundColor = DispatchGroupColor_Title(groupColor);
        }

        private Color DispatchGroupColor_Title(GroupColor groupColor)
        {
            var contentColor = DispatchGroupColor_Content(groupColor);
            contentColor.r *= 0.8f;
            contentColor.g *= 0.8f;
            contentColor.b *= 0.8f;

            return contentColor;
        }
    }
}
