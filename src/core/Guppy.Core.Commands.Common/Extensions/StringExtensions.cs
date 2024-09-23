using Guppy.Core.Common.Extensions.System;

namespace Guppy.Core.Commands.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerCaseFirstLetter(this string input)
        {
            return char.ToLower(input[0]) + input.Substring(1);
        }

        public static string ToCommandName(this string input)
        {
            return input.ToLowerCaseFirstLetter().TrimEnd("Command");
        }

        public static string ToOptionName(this string input)
        {
            return $"--{input.ToLowerCaseFirstLetter()}";
        }

        public static string ToArgumentName(this string input)
        {
            return input.ToLowerCaseFirstLetter();
        }
    }
}
