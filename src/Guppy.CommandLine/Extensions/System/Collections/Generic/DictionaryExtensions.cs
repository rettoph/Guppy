using Guppy.CommandLine;
using Guppy.CommandLine.Builders;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        internal static Dictionary<Type, Command> Build(this Dictionary<Type, CommandBuilder> builders)
        {
            Dictionary<Type, Command> commands = new Dictionary<Type, Command>();

            foreach(CommandBuilder builder in builders.Values)
            {
                commands.Add(builder.Type, builder.Build());
            }

            foreach(Type type in commands.Keys)
            {
                foreach(CommandBuilder subDefinition in builders.Values.Where(d => d.Parent is not null && d.Parent.GetType() == type))
                {
                    commands[type].AddCommand(commands[subDefinition.Type]);
                }
            }

            return commands;
        }
    }
}
