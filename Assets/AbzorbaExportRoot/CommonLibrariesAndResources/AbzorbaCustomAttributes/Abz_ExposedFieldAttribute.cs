using System;
using System.Diagnostics;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> This attribute is used to expose a serialized property to the AbzEditor_EditorWindow_ExposedFieldController. </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class Abz_ExposedFieldAttribute : Attribute 
    {
    }
}