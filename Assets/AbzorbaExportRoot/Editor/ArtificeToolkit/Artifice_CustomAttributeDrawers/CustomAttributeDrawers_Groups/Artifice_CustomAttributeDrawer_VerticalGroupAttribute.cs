using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.ArtificeAttributes;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Custom drawer for <see cref="VerticalGroupAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(VerticalGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_VerticalGroupAttribute : Artifice_CustomAttributeDrawer_BoxGroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_VerticalGroup);
    }
}
