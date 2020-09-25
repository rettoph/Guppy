using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.IO.Commands.Extensions
{
    public static class ICommandContextExtensions
    {
        /// <summary>
        /// Parse a given argument array and return a new ICommand instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Command Build(this ICommandContext context, String[] args, Byte position)
        {
            Int32 offset = 0;
            Dictionary<String, Object> parsedArgs = new Dictionary<String, Object>();
            String cur;
            ArgContext arg;

            while(position + offset < args.Length)
            {
                cur = args[position + offset];

                if (cur[0] != CommandService.ArgumentIdentifier)
                    throw new Exception($"Unexpected argument identifier. Please use '{CommandService.ArgumentIdentifier}'");

                // Trim the argument identifier
                cur = cur.Substring(1);
                // Identify the inputed argument.
                arg = context.Arguments.First(ac => ac.Name == cur || (cur.Length == 1 && ac.Aliases.Contains(cur[0])));

                // Parse the argument input...
                parsedArgs.Add(arg.Name, arg.Type.Parse(args[position + ++offset]));

                offset++;
            }

            // Ensure that all required args were recieved...
            if(context.Arguments.Any(ac => ac.Required && !parsedArgs.ContainsKey(ac.Name)))
                throw new Exception("Missing required argument!");

            return new Command()
            {
                Context = context,
                Data = context.BuildData(parsedArgs)
            };
        }
    }
}
