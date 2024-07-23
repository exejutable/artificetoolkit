using UnityEditor;

// ReSharper disable InconsistentNaming

namespace AbzorbaExportRoot.Editor.ArtificeToolkit
{
    public class ArtificeEditorWindow : EditorWindow
    {
        private void CreateGUI()
        {
            var drawer = new ArtificeDrawer();

            // Get reference to serializedObject and simply call 
            var serializedObject = new SerializedObject(this);
            rootVisualElement.Add(drawer.CreateInspectorGUI(serializedObject));

            // Add padding here to avoid extra style file
            rootVisualElement.style.paddingBottom = 5;
            rootVisualElement.style.paddingLeft = 5;
            rootVisualElement.style.paddingTop = 5;
            rootVisualElement.style.paddingBottom = 5;
        }
    }
}
