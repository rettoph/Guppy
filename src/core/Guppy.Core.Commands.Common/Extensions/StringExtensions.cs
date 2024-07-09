namespace Guppy.Core.Commands.Common.Extensions
{
    internal static class StringExtensions
    {
        public static string ToLowerCaseFirstLetter(this string input)
        {
            return char.ToLower(input[0]) + input.Substring(1);
        }
    }
}
