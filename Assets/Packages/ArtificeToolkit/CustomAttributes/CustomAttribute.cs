using System;
using System.Diagnostics;

namespace Packages.ArtificeToolkit.CustomAttributes
{
    /// <summary>Customizes how a field is rendered in the inspector.</summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class CustomAttribute : Attribute
    {
    }
}