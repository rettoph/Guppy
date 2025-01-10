namespace Guppy.Core.Common.Extensions.System
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
            {
                return source;
            }


            return source.Remove(source.LastIndexOf(value));
        }

        public static bool IsNullOrEmpty(this string? value) => value is null || value == string.Empty;

        public static bool IsNotNullOrEmpty(this string? value) => value is not null && value != string.Empty;
    }
}