namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary>
    /// This attribute allows a "dummy" variable to show in the inspector as a button.
    /// The name of the method is required and any number of given parameters found in the same scope.
    /// </summary>
    public class Abz_ButtonAttribute : Abz_CustomAttribute
    {
        public readonly string MethodName;
        public readonly string[] ParameterNames;
        
        public Abz_ButtonAttribute(string methodName, params string[] parameters)
        {
            MethodName = methodName;
            ParameterNames = parameters;
        }
    }
}
