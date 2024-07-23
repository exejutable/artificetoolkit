namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    public class Abz_RangeAttribute : Abz_CustomAttribute
    {
        public readonly float MinValue;
        public readonly float MaxValue;

        public Abz_RangeAttribute(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
