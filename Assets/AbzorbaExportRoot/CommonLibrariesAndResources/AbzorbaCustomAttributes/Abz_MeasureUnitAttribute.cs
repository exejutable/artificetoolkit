using UnityEngine;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Appends to property a string with the unit name </summary>
    public class Abz_MeasureUnitAttribute : Abz_CustomAttribute
    {
        public readonly string UnitName;
        
        public Abz_MeasureUnitAttribute(string unitName)
        {
            UnitName = unitName;
        }
    }
}
