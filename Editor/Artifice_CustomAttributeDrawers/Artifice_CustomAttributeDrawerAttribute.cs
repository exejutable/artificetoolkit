using System;
using ArtificeToolkit.Attributes;
using UnityEngine;

namespace ArtificeToolkit.Editor.Artifice_CustomAttributeDrawers
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