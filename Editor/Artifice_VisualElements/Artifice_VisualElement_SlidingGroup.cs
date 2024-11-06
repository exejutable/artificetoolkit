using ArtificeToolkit.Editor;
using ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Editor.Artifice_VisualElements
{
    /// <summary>
    /// This element provides a sliding container which is enabled or disabled by a button.
    /// </summary>
    public class Artifice_VisualElement_SlidingGroup : Artifice_VisualElement_Group
    {
        public bool IsExpanded;
        
        private Button _handeElem;

        private ValueAnimation<float> _valueAnimation = new ValueAnimation<float>();
        
        public Artifice_VisualElement_SlidingGroup() : base()
        {
            styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            IsExpanded = false;
            SlideOff();
            
            // Default content is created in base constructor.
            // Make default container's display None at beginning to not affect the layout composition. Otherwise it seems to enlarge the container of the inspector.
            DefaultContentContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            
            // Create handle button
            _handeElem = new Button(ToggleSlidingContainer);
            _handeElem.text = "Sliding Group";
            _handeElem.AddToClassList("handle");
            hierarchy.Insert(0, _handeElem);
        }

        public override void SetTitle(string title)
        {
            _handeElem.text = title;
        }

        private void ToggleSlidingContainer()
        {
            if (IsExpanded)
                SlideOff();
            else
                SlideOn();

            IsExpanded = !IsExpanded;
        }

        private void SlideOn()
        {
            // Stop potential previous animation
            if(_valueAnimation != null && _valueAnimation.isRunning)
                _valueAnimation?.Stop();
            
            // Make visible and flex.
            DefaultContentContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            DefaultContentContainer.style.position = new StyleEnum<Position>(Position.Absolute);
            
            // Begin animation!
            var width = resolvedStyle.width;
            _valueAnimation = DefaultContentContainer.experimental.animation.Start(1f, 0f, 400, (elem, value) =>
            {
                elem.style.translate = new StyleTranslate(new Translate(width * value, 0f));
            });
        }

        private void SlideOff()
        {
            // Stop potential previous animation
            if(_valueAnimation != null && _valueAnimation.isRunning)
                _valueAnimation?.Stop();
            
            // Make visible and flex.
            DefaultContentContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            DefaultContentContainer.style.position = new StyleEnum<Position>(Position.Absolute);
            
            // Begin animation!
            var width = resolvedStyle.width;
            _valueAnimation = DefaultContentContainer.experimental.animation.Start(0f, 1f, 400, (elem, value) =>
            {
                elem.style.translate = new StyleTranslate(new Translate(width * value, 0f));
            });
        }
    }
}