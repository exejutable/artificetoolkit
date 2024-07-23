using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.ArtificeAttributes;
using UnityEngine;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers
{
    /// <summary> This attribute marks specific classes as drawers for <see cref="CustomAttribute"/> </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Artifice_CustomAttributeDrawerAttribute : PropertyAttribute
    {
        public Type Type { get; private set; }

        public Artifice_CustomAttributeDrawerAttribute(Type type)
        {
            Type = type;
        }
    }
}