namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Bounds the property's minimum value to a given value </summary>
    public class Abz_MinValueAttribute : Abz_CustomAttribute
    {
        public float Value;
        
        public Abz_MinValueAttribute(float value)
        {
            Value = value;
        }
    }
}
