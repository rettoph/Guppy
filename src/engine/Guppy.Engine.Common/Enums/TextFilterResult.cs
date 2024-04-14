namespace Guppy.Engine.Common.Enums
{
    public enum TextFilterResult
    {
        None = 0,
        NotMatched = 1,
        Matched = 2,
    }

    public static class TextFilterResultExtensions
    {
        public static TextFilterResult Max(this TextFilterResult item1, TextFilterResult item2)
        {
            if (item1 > item2)
            {
                return item1;
            }

            return item2;
        }
    }
}
