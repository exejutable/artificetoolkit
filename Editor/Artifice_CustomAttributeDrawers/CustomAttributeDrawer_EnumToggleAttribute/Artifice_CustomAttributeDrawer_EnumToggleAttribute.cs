using System;
using System.Collections.Generic;
using System.Reflection;
using ArtificeToolkit.Attributes;
using ArtificeToolkit.AEditor.VisualElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_EnumToggleAttribute
{
    /// <summary> Custom VisualAttribute drawer for <see cref="EnumToggleAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(EnumToggleAttribute))]
    public class Artifice_CustomAttributeDrawer_EnumToggleAttribute : Artifice_CustomAttributeDrawer
    {
        #region FIELDS

        public override bool IsReplacingPropertyField { get; } = true;

        private readonly Dictionary<object, AbzSlotEditor_LabeledButtonState> _buttonStateMap = new Dictionary<object, AbzSlotEditor_LabeledButtonState>();

        private SerializedProperty _property;
        private EnumToggleAttribute _attribute;
        private bool _useFlags;
        private int _intForAllFlags;
        private bool _disposed;
        
        #endregion

        /// <summary> Utility class for _buttonStateMap </summary>
        private class AbzSlotEditor_LabeledButtonState
        {
            public readonly Artifice_VisualElement_LabeledButton Button;

            private readonly bool _checkExactEquality;
            private readonly int _representedEnumFlags;

            public AbzSlotEditor_LabeledButtonState(Artifice_VisualElement_LabeledButton button, int representedEnumFlags, bool checkExactEquality = false)
            {
                Button = button;
                _representedEnumFlags = representedEnumFlags;
                _checkExactEquality = checkExactEquality;
            }

            public bool IsPressedRelativeTo(Enum value)
            {
                if (_checkExactEquality)
                {
                    var intValue = Convert.ToInt64(value);
                    return _representedEnumFlags == intValue;
                }
                else
                    return Artifice_Utilities.AreEqual(_representedEnumFlags, value);
            }
        }
        
        /* Base Drawer Method*/
        public override VisualElement OnPropertyGUI(SerializedProperty property)
        {
            _property = property;
            _attribute = (EnumToggleAttribute)Attribute;
            InitializeIntForAllFlags();
            DetectAndUpdateUseFlags();

            _property.serializedObject.Update();

            // Create main container for field
            var propertyField = new VisualElement();
            propertyField.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            propertyField.AddToClassList("main-container");

            // Create Label for enum if not hidden
            if (_attribute.HideLabel == false)
                propertyField.Add(BuildPropertyLabel());

            // Add toggle buttons
            propertyField.Add(BuildToggleButtons());
            UpdateAllButtonVisuals();

            // Subscribe to undo
            Undo.undoRedoPerformed += UpdateAllButtonVisuals;
            
            return propertyField;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            
            base.Dispose(disposing);
            Undo.undoRedoPerformed -= UpdateAllButtonVisuals;

            _disposed = true;
        }

        #region Build UI

        private VisualElement BuildPropertyLabel()
        {
            // Get reference to object enum
            var target = _property.GetTarget<Enum>();

            // Create label
            var label = new Label(_property.displayName);
            label.AddToClassList("label");
            return label;
        }

        private VisualElement BuildToggleButtons()
        {
            // Get reference to object enum
            var target = _property.GetTarget<Enum>();

            var container = new VisualElement();
            container.AddToClassList("buttons-container");

            // If we have flagsAdd All button
            if (_useFlags)
                container.Add(BuildAllAndNoneButtons());

            // Create enum toggle buttons
            foreach (var option in Enum.GetValues(target.GetType()))
            {
                container.Add(BuildToggleButton(option.ToString(), Convert.ToInt32(option)));
            }


            return container;
        }

        private VisualElement BuildToggleButton(string name, int representedEnumFlag)
        {
            var button = new Artifice_VisualElement_LabeledButton(name, () => { });
            button.AddToClassList("toggle-button");
            button.SetAction(() =>
            {
                UpdateEnumValue(representedEnumFlag);
                UpdateAllButtonVisuals();
            });

            // Add to dictionary
            _buttonStateMap.Add(representedEnumFlag, new AbzSlotEditor_LabeledButtonState(button, representedEnumFlag));

            return button;
        }

        private VisualElement BuildAllAndNoneButtons()
        {
            var container = new VisualElement();
            container.AddToClassList("extra-control-buttons-container");

            var allButton = new Artifice_VisualElement_LabeledButton("All", () => { });
            allButton.AddToClassList("toggle-button");
            allButton.AddToClassList("extra-control-button");
            allButton.Bind(_property.serializedObject);
            allButton.SetAction(() =>
            {
                if (_property.enumValueFlag != _intForAllFlags)
                {
                    // A) Set all as selected!
                    _property.enumValueFlag = _intForAllFlags;
                }
                else
                {
                    // B) If all were selected, unselect them
                    _property.enumValueFlag = 0;
                }

                _property.serializedObject.ApplyModifiedProperties();
                _property.serializedObject.Update();

                UpdateAllButtonVisuals();
            });
            container.Add(allButton);

            // Add to dictionary
            _buttonStateMap.Add(_intForAllFlags, new AbzSlotEditor_LabeledButtonState(allButton, _intForAllFlags, true));

            return container;
        }

        #endregion

        private void InitializeIntForAllFlags()
        {
            // Enum type
            var type = _property.GetTarget<object>().GetType();
            var count = Enum.GetNames(type).Length;
            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                var intValue = Convert.ToInt32(value);
                _intForAllFlags += intValue;
            }

            // for (int i = 0; i < count; i++)
            //     _intForAllFlags += (int)Mathf.Pow(2, i);
        }

        private void DetectAndUpdateUseFlags()
        {
            var type = _property.GetTarget<object>().GetType();
            if (type.GetCustomAttribute(typeof(FlagsAttribute)) != null)
                _useFlags = true;
            else
                _useFlags = false;
        }

        private void UpdateEnumValue(int value)
        {
            // Easier to debug instead of inline
            var v1 = value;
            var v2 = Convert.ToInt32(_property.enumValueFlag);

            if (_useFlags)
            {
                v2 ^= v1;
            }
            else
            {
                // Normal enum can only have one representedEnumFlag at a single time.
                v2 = v1;
            }

            _property.enumValueFlag = Convert.ToInt32(v2);
            _property.serializedObject.ApplyModifiedProperties();
        }

        private void UpdateAllButtonVisuals()
        {
            var target = _property.GetTarget<Enum>();

            foreach (var pair in _buttonStateMap)
            {
                var state = pair.Value;

                // Add pressed or not
                if (state.IsPressedRelativeTo(target))
                    state.Button.Query<Button>().First().AddToClassList("toggle-button-pressed");
                else
                    state.Button.Query<Button>().First().RemoveFromClassList("toggle-button-pressed");
            }
        }
    }
}