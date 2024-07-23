namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Shows in the inspector an info box the parameterized message </summary>
    public class Abz_InfoBoxAttribute : Abz_CustomAttribute
    {
        public string Message;
        public InfoMessageType Type;
        
        public enum InfoMessageType
        {
            Info, Warning, Error, None
        }
        
        public Abz_InfoBoxAttribute(string message)
        {
            Message = message;
            Type = InfoMessageType.Info;
        }

        public Abz_InfoBoxAttribute(string message, InfoMessageType type)
        {
            Message = message;
            Type = type;
        }
    }
}