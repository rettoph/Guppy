using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Guppy.IO.Commands.Extensions
{
    public static class CommandContextExtensions
    {
        public static void ParseArguments(this Command command, String[] input, Int32 position, Dictionary<String, Object> args)
        {
            args.Clear();

            while (position < input.Length)
                command.AddNextArgument(input, ref position, ref args);

            // Ensure that all required args were recieved...
            if (command.Arguments.Any(ac => ac.Required && !args.ContainsKey(ac.Identifier)))
            {
                var missing = command.Arguments.Where(ac => ac.Required && !args.ContainsKey(ac.Identifier));
                throw new MissingMemberException($"Missing arguments required: {String.Join(" ", missing.Select(ac => CommandService.ArgumentIdentifier + ac.Identifier).ToArray())}.");
            }
        }

        /// <summary>
        /// Find the next argument identifier & its value
        /// then add it do the recieved args collection
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static void AddNextArgument(this Command command, String[] input, ref Int32 position, ref Dictionary<String, Object> args)
        {
            if (input[position][0] != CommandService.ArgumentIdentifier)
                throw new Exception($"Unexpected argument identifier. Please use '{CommandService.ArgumentIdentifier}'");

            String identifier;
            String value;
            Int32 index;

            String next = input[position].Substring(1);

            if ((index = next.IndexOf('=')) > 0)
            { // There is an equal sign, thus no space seperator
                identifier = next.Substring(0, index);
                value = Regex.Unescape(next.Substring(++index, next.Length - index));
            }
            else
            {
                identifier = next;
                value = Regex.Unescape(input[++position]);
            }

            if(value[0] == '"')
            { // If a quote is returned...
                while(value[value.Length - 1] != '"')
                {
                    value += ' ' + Regex.Unescape(input[++position]);
                }

                value = value.Trim('"');
            }

            // Identify which argument is being returned & parse the String value
            try
            {
                var argContext = command.Arguments.First(ac => ac.Identifier == identifier || (identifier.Length == 1 && ac.Aliases.Contains(identifier[0])));
                args.Add(argContext.Identifier, argContext.Type.Parse(value));
            }
            catch(InvalidOperationException e)
            {
                throw new ArgumentException($"Unknown argument identifier or aliase '{identifier}'.");
            }

            position++;
        }
    }
}
