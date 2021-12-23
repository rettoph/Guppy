using Guppy.CommandLine.Arguments;
using Guppy.CommandLine.Services;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine
{
    public abstract class CommandDefinition
    {
        public abstract String Name { get; }
        public virtual String Description { get; } = default;
        public virtual String[] Aliases { get; } = Array.Empty<String>();
        public virtual Option[] Options { get; } = Array.Empty<Option>();
        public virtual Argument[] Arguments { get; } = Array.Empty<Argument>();

        public CommandDefinition()
        {

        }

        public virtual ICommandHandler CreateCommandHandler(CommandService commands)
        {
            return CommandHandler.Create(() =>
            {
                //
            });
        }
    }
}
