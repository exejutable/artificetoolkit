using ArtificeToolkit.AEditor.Resources;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArtificeToolkit.AEditor.VisualElements
{
    public class Artifice_VisualElement_InfoBox : VisualElement
    {
        private readonly Image _image;
        private readonly Label _label;

        private Artifice_VisualElement_InfoBox()
        {
            // Load stylesheet
            styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            
            // Load container style
            AddToClassList("info-box-container");
            
            // Add image
            _image = new Image();
            _image.AddToClassList("image");
            Add(_image);

            // Add label
            _label = new Label();
            _label.AddToClassList("label");
            _label.style.whiteSpace = WhiteSpace.Normal;
            Add(_label);
        }
        public Artifice_VisualElement_InfoBox(string message) : this()
        {
            _label.text = message;
            _image.sprite = Artifice_SCR_CommonResourcesHolder.instance.CommentIcon;
        }
        public Artifice_VisualElement_InfoBox(string message, Sprite sprite) : this()
        {
            _image.sprite = sprite;
            _label.text = message;
        }
    }
}
