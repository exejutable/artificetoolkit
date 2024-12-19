namespace ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary> Inherits from <see cref="Artifice_VisualElement_Group"/> but offers foldout capabilities </summary>
    public class Artifice_VisualElement_VerticalGroup : Artifice_VisualElement_Group
    {
        public Artifice_VisualElement_VerticalGroup() : base()
        {
            DefaultContentContainer.styleSheets.Add(Artifice_Utilities.GetStyle(GetType()));
        }
    }
}