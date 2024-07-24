namespace CustomAttributes
{
    /// <summary> Appends to property a string with the unit name </summary>
    public class MeasureUnitAttribute : CustomAttribute
    {
        public readonly string UnitName;
        
        public MeasureUnitAttribute(string unitName)
        {
            UnitName = unitName;
        }
    }
}
