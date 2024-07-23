namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Adds space in the given direction. </summary>
    public class Abz_SpaceAttribute : Abz_CustomAttribute
    {
        public readonly int ValueTop;
        public readonly int ValueBottom;
        public readonly int ValueLeft;
        public readonly int ValueRight;

        public Abz_SpaceAttribute(int top)
        {
            ValueTop = top;
        }

        public Abz_SpaceAttribute(int top, int bottom)
        {
            ValueTop = top;
            ValueBottom = bottom;
        }
        
        public Abz_SpaceAttribute(int top, int bottom, int left, int right)
        {
            ValueTop = top;
            ValueBottom = bottom;
            ValueLeft = left;
            ValueRight = right;
        }
    }
}
