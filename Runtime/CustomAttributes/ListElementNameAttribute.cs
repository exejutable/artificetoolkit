using ArtificeToolkit.Attributes;

namespace CustomAttributes
{
    public class ListElementNameAttribute : CustomAttribute
    {
        public readonly string FieldName;

        public ListElementNameAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
