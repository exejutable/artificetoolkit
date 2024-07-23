using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Custom VisualAttribute drawer for <see cref="Abz_FoldoutGroupAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(Abz_FoldoutGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_FoldoutGroupAttribute : Artifice_CustomAttributeDrawer_BoxGroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_FoldoutGroup);
    }
}