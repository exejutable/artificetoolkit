using System;
using System.Diagnostics;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary>Customizes how a field is rendered in the inspector.</summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class Abz_CustomAttribute : Attribute
    {
    }
}