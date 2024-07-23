using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Custom drawer for <see cref="Abz_VerticalGroupAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(Abz_VerticalGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_VerticalGroupAttribute : Artifice_CustomAttributeDrawer_BoxGroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_VerticalGroup);
    }
}
