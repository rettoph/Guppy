namespace Guppy.Engine.Common.Enums
{
    public enum TextFilterResultEnum
    {
        None = 0,
        NotMatched = 1,
        Matched = 2,
    }

    public static class TextFilterResultExtensions
    {
        public static TextFilterResultEnum Max(this TextFilterResultEnum item1, TextFilterResultEnum item2)
        {
            if (item1 > item2)
            {
                return item1;
            }

            return item2;
        }
    }
}