namespace AbzorbaExportRoot.CommonLibrariesAndResources.ArtificeAttributes
{
    /// <summary> This attribute receives a method name which invokes after the property tracks a change.
    /// Temporary attribute, until expression are added in the Artifice system. </summary>
    public class InvokeOnValueChangeAttribute : CustomAttribute
    {
        public readonly string MethodName;

        public InvokeOnValueChangeAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}
