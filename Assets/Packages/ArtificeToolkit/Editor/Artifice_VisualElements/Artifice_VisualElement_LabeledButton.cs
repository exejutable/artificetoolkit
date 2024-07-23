using System;
using UnityEngine.UIElements;

namespace Packages.ArtificeToolkit.Editor.Artifice_VisualElements
{
    /// <summary> This class can be used as a button with a label centered inside of it </summary>
    public class Artifice_VisualElement_LabeledButton : Button
    {
        public Artifice_VisualElement_LabeledButton(string label, Action callback)
        {
            styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            AddToClassList("labeled-button");
            
            // Create label
            var labelField = new Label(label);
            Add(labelField);

            // Set callback as action
            SetAction(callback);
        }

        public void SetAction(Action callback)
        {
            clicked += callback;
        }
    }
}