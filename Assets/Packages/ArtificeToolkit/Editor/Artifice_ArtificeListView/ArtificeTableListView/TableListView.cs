using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace Packages.ArtificeToolkit.Editor.Artifice_ArtificeListView.ArtificeTableListView
{
    /// <summary> This class is used to render arrays and lists in a way the supports CustomAttributes and offers more functionality than Unity's default lists. </summary>
    /// <remarks><see cref="ArtificeDrawer"/></remarks>
    public class TableListView : AbstractListView
    {
        private class FieldColumnData
        {
            public string Name;
            public float WidthPercent;

            public VisualElement HeaderElement;
            public List<VisualElement> FieldElements;
            
            public FieldColumnData(string name)
            {
                Name = name;
                WidthPercent = -1;

                FieldElements = new List<VisualElement>();
            }

            public void Refresh()
            {
                HeaderElement.style.width = Length.Percent(WidthPercent);
                foreach (var fieldElem in FieldElements)
                    fieldElem.style.width = Length.Percent(WidthPercent);
            }
        }
        
        #region FIELDS
        
        private readonly List<FieldColumnData> _fieldColumns;
        private bool _disposed;

        private VisualElement _headerContainer;
        private List<VisualElement> _dragHandlers;
        
        private bool _isClicked;
        private int _selectedLeftColumnIndex;
        private VisualElement _selectedColumnHandler;
        
        #endregion
        
        public TableListView()
        {
            _fieldColumns = new List<FieldColumnData>();
            _dragHandlers = new List<VisualElement>();
            
            styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
            
            RegisterCallback<MouseMoveEvent>(OnMouseMoveEventHandler);
            RegisterCallback<MouseUpEvent>(OnMouseUpEventHandler);
        }
        
        protected override VisualElement BuildPrePropertyUI(SerializedProperty property)
        {
            _fieldColumns.Clear();
            _dragHandlers.Clear();
            
            _headerContainer = new VisualElement();
            _headerContainer.AddToClassList("header-container");

            var childType = property.GetArrayChildrenType();
            
            // Create field columns and label elements
            var fields = childType.GetFields();
            foreach (var field in fields)
            {
                var data = new FieldColumnData(field.Name);
                data.WidthPercent = 100f / fields.Length; 
                _fieldColumns.Add(data);
                
                var labelContainer = new VisualElement();
                labelContainer.AddToClassList("column-label-container");
                labelContainer.style.width = Length.Percent(data.WidthPercent);
                _headerContainer.Add(labelContainer);
                data.HeaderElement = labelContainer;
                
                var label = new Label(field.Name);
                labelContainer.Add(label);
            }
            
            // Create in between elements
            var percentTotal = 0f;
            for (var i = 0; i < _fieldColumns.Count - 1; i++)
            {
                // Create handler element
                var dragHandler = new VisualElement();
                dragHandler.AddToClassList("drag-handler");
                _dragHandlers.Add(dragHandler);
                
                // Add header container
                _headerContainer.Add(dragHandler);

                // Set position of drag handler based on previous width percents
                percentTotal += _fieldColumns[i].WidthPercent;
                dragHandler.style.left = Length.Percent(percentTotal);
                
                // Set callbacks
                var capturedI = i;
                dragHandler.RegisterCallback<MouseDownEvent>(evt =>
                {
                    _selectedColumnHandler = dragHandler;
                    OnMouseDownEventHandler(capturedI, evt);
                });
            }
                
            return _headerContainer;
        }
        
        protected override VisualElement BuildPropertyFieldUI(SerializedProperty property, int index)
        {
            // Iterate
            var propertyContainer = new VisualElement();
            propertyContainer.AddToClassList("property-container");

            foreach (var field in _fieldColumns)
            {
                // Create field for sub-property
                var fieldContainer = new VisualElement();
                fieldContainer.AddToClassList("field-container");
                propertyContainer.Add(fieldContainer);
                
                // Create sub property field
                var subProperty = property.FindPropertyRelative(field.Name);
                var subPropertyField = ArtificeDrawer.CreatePropertyGUI(subProperty, true);
                subPropertyField = ArtificeDrawer.CreateCustomAttributesGUI(subProperty, subPropertyField, ChildrenInjectedCustomAttributes);
                subPropertyField.AddToClassList("sub-property-field");
                
                // Add subPropertyField to its respective list.
                field.FieldElements.Add(fieldContainer);

                // Set width from field.Width
                fieldContainer.style.width = Length.Percent(field.WidthPercent);
                
                fieldContainer.Add(subPropertyField);
            }
            
            return propertyContainer;
        }

        private void OnMouseDownEventHandler(int leftColumnIndex, MouseDownEvent evt)
        {
            _isClicked = true;
            _selectedLeftColumnIndex = leftColumnIndex;
        }
        private void OnMouseMoveEventHandler(MouseMoveEvent evt)
        {
            if (!_isClicked)
                return;
            
            // Assert selected
            Debug.Assert(
                _selectedLeftColumnIndex >= 0 && _selectedLeftColumnIndex < _fieldColumns.Count - 1,
                $"SelectedLeftColumnIndex must be between 0 and {+_fieldColumns.Count - 1}"
            );

            // Get mouse deltaX;
            var mouseDelta = evt.mouseDelta;
            
            // Analogous percent
            var maxSize = _headerContainer.resolvedStyle.width;
            var percentChange = 100f * mouseDelta.x / maxSize;

            // Change width percents and refresh
            _fieldColumns[_selectedLeftColumnIndex].WidthPercent += percentChange;
            _fieldColumns[_selectedLeftColumnIndex + 1].WidthPercent -= percentChange;
            _fieldColumns[_selectedLeftColumnIndex].Refresh();
            _fieldColumns[_selectedLeftColumnIndex + 1].Refresh();
            
            // Update drag handler position
            var dragHandler =_dragHandlers[_selectedLeftColumnIndex];
            dragHandler.style.left = Length.Percent(dragHandler.style.left.value.value + percentChange);
        }
        private void OnMouseUpEventHandler(MouseUpEvent evt)
        {
            // Save load
            SavePersistedData();
            
            _isClicked = false;
            _selectedLeftColumnIndex = -1;
        }
        
        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    ArtificeDrawer.Dispose();
                }

                _disposed = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        #region Save/Load Persistence
        
        public override void SavePersistedData()
        {
            // Set foreach field, the width
            foreach (var fieldColumn in _fieldColumns)
                Artifice_SCR_ArtificePersistedData.instance.SaveData(ViewPersistenceKey, fieldColumn.Name, fieldColumn.WidthPercent.ToString());
        }

        public override void LoadPersistedData()
        {
            foreach (var fieldColumn in _fieldColumns)
            {
                var savedWidth = Artifice_SCR_ArtificePersistedData.instance.LoadData(ViewPersistenceKey, fieldColumn.Name);
                if(float.TryParse(savedWidth, out var width))
                {
                    fieldColumn.WidthPercent = width; 
                    fieldColumn.Refresh();
                }
            }

            var percentTotal = 0f;
            for(var i = 0; i < _dragHandlers.Count; i++)
            {
                percentTotal += _fieldColumns[i].WidthPercent;
                _dragHandlers[i].style.left = Length.Percent(percentTotal);
            }
        }

        protected override void SetViewPersistenceKey(SerializedProperty property)
        {
            ViewPersistenceKey = property.GetArrayChildrenType().ToString();
        }

        #endregion
    }
}