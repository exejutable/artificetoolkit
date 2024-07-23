using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Custom VisualAttribute drawer for <see cref="Abz_BoxGroupAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(Abz_BoxGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_BoxGroupAttribute : Artifice_CustomAttributeDrawer_GroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_BoxGroup);
    }
}