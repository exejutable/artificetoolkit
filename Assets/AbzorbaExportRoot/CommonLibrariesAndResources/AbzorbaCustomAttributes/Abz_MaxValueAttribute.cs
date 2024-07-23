namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Bounds the property's max value to a given value </summary>
    public class Abz_MaxValueAttribute : Abz_CustomAttribute
    {
        public float Value;
        
        public Abz_MaxValueAttribute(float value)
        {
            Value = value;
        }
    }
}
