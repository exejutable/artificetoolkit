namespace CustomAttributes
{
    /// <summary>
    /// This attribute allows a "dummy" variable to show in the inspector as a button.
    /// The name of the method is required and any number of given parameters found in the same scope.
    /// </summary>
    public class ButtonAttribute : CustomAttribute
    {
        public readonly string MethodName;
        public readonly string[] ParameterNames;
        
        public ButtonAttribute(string methodName, params string[] parameters)
        {
            MethodName = methodName;
            ParameterNames = parameters;
        }
    }
}
