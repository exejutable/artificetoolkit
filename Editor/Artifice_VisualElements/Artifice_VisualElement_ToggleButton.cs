using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArtificeToolkit.Editor.VisualElements
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
        
        private bool _isPressed;
        private readonly Sprite _enabledSprite;
        private readonly Sprite _disabledSprite;
        
        #endregion

        private Artifice_VisualElement_ToggleButton()
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
            Add(_image);
            
            // Create label
            _label = new Label();
            _label.AddToClassList("toggle-label");
            Text = "";
            Add(_label);
        }
        public Artifice_VisualElement_ToggleButton(string title, Sprite sprite = null, bool isPressed = false) : this()
        {
            // Set private sprites
            _enabledSprite = _disabledSprite = sprite;

            // Set title
            Text = title;
            
            // Call visual update once to be consistent
            SetState(isPressed);
        }

        public Artifice_VisualElement_ToggleButton(string title, Sprite enabledSprite, Sprite disabledSprite, bool isPressed) : this()
        {
            // Set private sprites
            this._enabledSprite = enabledSprite;
            this._disabledSprite = disabledSprite;

            // Set properties
            Text = title;
            
            // Call visual update once to be consistent
            SetState(isPressed);
        }
        
        public void SetState(bool state)
        {
            _isPressed = state;
            if (_isPressed)
            {
                Sprite = _enabledSprite;
                RemoveFromClassList("toggle-button-not-pressed");
                AddToClassList("toggle-button-pressed");
            }
            else
            {
                Sprite = _disabledSprite;
                AddToClassList("toggle-button-not-pressed");
                RemoveFromClassList("toggle-button-pressed");
            }
            
            // Play animation of transition
            if (_enabledSprite != _disabledSprite)
            {
                _image.experimental.animation.Start(0f, 1f, 200, (elem, value) =>
                {
                    var mappedValue = Map(value, 0f, 1f, -1f, 1f);
                    elem.style.scale = new StyleScale(new Vector2(mappedValue, mappedValue));
                });
            }
        }
        
        private void OnMouseDownEvent(MouseDownEvent evt)
        {
            SetState(!_isPressed);
            OnButtonPressed?.Invoke(_isPressed);
        }
        
        #region Utilities
        
        private static float Map (float value, float from1, float to1, float from2, float to2) 
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        #endregion
    }
}
