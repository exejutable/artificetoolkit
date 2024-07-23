namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> This attribute receives a method name which invokes after the property tracks a change.
    /// Temporary attribute, until expression are added in the Artifice system. </summary>
    public class Abz_InvokeOnValueChangeAttribute : Abz_CustomAttribute
    {
        public readonly string MethodName;

        public Abz_InvokeOnValueChangeAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}
