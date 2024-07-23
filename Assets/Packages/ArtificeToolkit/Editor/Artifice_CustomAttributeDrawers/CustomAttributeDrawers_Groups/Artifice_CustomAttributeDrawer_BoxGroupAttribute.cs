using System;
using Packages.ArtificeToolkit.CustomAttributes;

namespace Packages.ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Custom VisualAttribute drawer for <see cref="BoxGroupAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(BoxGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_BoxGroupAttribute : Artifice_CustomAttributeDrawer_GroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_BoxGroup);
    }
}