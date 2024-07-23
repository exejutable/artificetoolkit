using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_VisualElements
{
    public class Artifice_VisualElement_ToggleButton : VisualElement
    {
        public string Text
        {
            get => _label.text;
            set
            {
                _label.text = value;
                
                if(value == null)
                    _label.AddToClassList("hide");
                else
                    _label.RemoveFromClassList("hide");
            }
        }
        public Sprite Sprite
        {
            get => _image.sprite;
            set
            {
                _image.sprite = value;
                
                if(value == null)
                    _image.AddToClassList("hide");
                else
                    _image.RemoveFromClassList("hide");
            }
        }

        #region FIELDS

        public Action<bool> OnButtonPressed;

        private readonly Label _label;
        private readonly Image _image;
        
        private bool _isPressed = false;
        
        #endregion

        public Artifice_VisualElement_ToggleButton(string title, Sprite sprite = null, bool isPressed = false)
        {
            // Get stylesheet
            styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            // Assign USS to self
            AddToClassList("toggle-button-container");
            AddToClassList("toggle-button-not-pressed");
            // Register click event
            RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
            
            // Create image
            _image = new Image();
            _image.AddToClassList("toggle-image");
            Sprite = sprite;
            Add(_image);
            
            // Create label
            _label = new Label();
            _label.AddToClassList("toggle-label");
            Text = title;
            Add(_label);
            
            // Call visual update once to be consistent
            SetState(isPressed);
        }

        public void SetState(bool state)
        {
            _isPressed = state;
            if (_isPressed)
            {
                RemoveFromClassList("toggle-button-not-pressed");
                AddToClassList("toggle-button-pressed");
            }
            else
            {
                AddToClassList("toggle-button-not-pressed");
                RemoveFromClassList("toggle-button-pressed");
            }
        }
        
        private void OnMouseDownEvent(MouseDownEvent evt)
        {
            SetState(!_isPressed);
            OnButtonPressed?.Invoke(_isPressed);
        }
    }
}
