using AbzorbaExportRoot.Editor.ArtificeToolkit.AbzEditor_ArtificeListView;
using UnityEditor;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_ArtificeListView
{
    /// <summary> This class is used to render arrays and lists in a way the supports CustomAttributes and offers more functionality than Unity's default lists. </summary>
    /// <remarks><see cref="ArtificeDrawer"/></remarks>
    public class ListView : AbstractListView
    {
        #region FIELDS
        
        private bool _disposed;

        #endregion

        protected override VisualElement BuildPropertyFieldUI(SerializedProperty property, int index)
        {
            // Create property's GUI with ArtificeDrawer
            var propertyField = ArtificeDrawer.CreatePropertyGUI(property);
            propertyField = ArtificeDrawer.CreateCustomAttributesGUI(property, propertyField, ChildrenInjectedCustomAttributes);
            propertyField.AddToClassList("property-field");

            return propertyField;
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
    }
}
