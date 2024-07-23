namespace AbzorbaExportRoot.CommonLibrariesAndResources.ArtificeAttributes
{
    public class RangeAttribute : CustomAttribute
    {
        public readonly float MinValue;
        public readonly float MaxValue;

        public RangeAttribute(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
