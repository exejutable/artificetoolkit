using UnityEditor;

// ReSharper disable InconsistentNaming

namespace ArtificeToolkit.Editor
{
    public class ArtificeEditorWindow : EditorWindow
    {
        private void CreateGUI()
        {
            var drawer = new ArtificeDrawer();
            drawer.SetSerializedPropertyFilter(property => property.name != "m_Script");

            // Get reference to serializedObject and simply call 
            var serializedObject = new SerializedObject(this);
            
            rootVisualElement.styleSheets.Add(Artifice_Utilities.GetStyle(typeof(ArtificeEditorWindow)));
            rootVisualElement.AddToClassList("root-visual-element");
            rootVisualElement.Add(drawer.CreateInspectorGUI(serializedObject));
        }
    }
}
