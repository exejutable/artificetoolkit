using System;
using System.Diagnostics;

namespace ArtificeToolkit.Attributes
{
    /// <summary>Customizes how a field is rendered in the inspector.</summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class CustomAttribute : Attribute
    {
    }
}