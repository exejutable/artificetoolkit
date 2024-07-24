namespace CustomAttributes
{
    public class RequiredAttribute : ValidatorAttribute
    {
        public string Message = "";
        
        public RequiredAttribute()
        {
            
        }

        public RequiredAttribute(string message)
        {
            Message = message;
        }
    }
}
