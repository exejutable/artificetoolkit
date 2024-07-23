using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.ArtificeAttributes;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Custom VisualAttribute drawer for <see cref="FoldoutGroupAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(FoldoutGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_FoldoutGroupAttribute : Artifice_CustomAttributeDrawer_BoxGroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_FoldoutGroup);
    }
}