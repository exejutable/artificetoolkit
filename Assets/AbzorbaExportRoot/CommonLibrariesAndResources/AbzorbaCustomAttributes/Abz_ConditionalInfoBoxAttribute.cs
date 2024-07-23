namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    public class Abz_ConditionalInfoBoxAttribute : Abz_InfoBoxAttribute
    {
        public readonly string PropertyName;
        public readonly object[] Values;

        public Abz_ConditionalInfoBoxAttribute(
            string message,
            string propertyName,
            params object[] values
        ) : base(message, InfoMessageType.Warning)
        {
            PropertyName = propertyName;
            Values = values;
        }
        
        public Abz_ConditionalInfoBoxAttribute(
            string message,
            InfoMessageType type,
            string propertyName,
            params object[] values
        ) : base(message, type)
        {
            PropertyName = propertyName;
            Values = values;
        }
    }
}
