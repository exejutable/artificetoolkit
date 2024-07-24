using System;
using CustomAttributes;

namespace Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawers_Groups
{
    /// <summary> Custom VisualAttribute drawer for <see cref="HorizontalGroupAttribute"/> </summary>
    [Artifice_CustomAttributeDrawer(typeof(HorizontalGroupAttribute))]
    public class Artifice_CustomAttributeDrawer_HorizontalGroupAttribute : Artifice_CustomAttributeDrawer_GroupAttribute
    {
        protected override Type VisualElementType { get; } = typeof(Artifice_VisualElement_HorizontalGroup);
    }
}