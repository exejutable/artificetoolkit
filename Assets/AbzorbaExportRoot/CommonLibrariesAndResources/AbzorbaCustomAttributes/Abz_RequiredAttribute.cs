namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    public class Abz_RequiredAttribute : Abz_ValidatorAttribute
    {
        public string Message = "";
        
        public Abz_RequiredAttribute()
        {
            
        }

        public Abz_RequiredAttribute(string message)
        {
            Message = message;
        }
    }
}
