using Guppy.Gaming.Services;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions
{
    public abstract class CommandDefinition
    {
        public virtual Type? Parent { get; } = null;
        public virtual string Name => this.GetType().Name.ToLower();
        public virtual string? Description { get; }
        public virtual string[] Aliases { get; } = Array.Empty<string>();
        public virtual Option[] Options { get; } = Array.Empty<Option>();
        public virtual Argument[] Arguments { get; } = Array.Empty<Argument>();

        public abstract ICommandHandler? Publisher(IBroker broker);

        public virtual Command BuildCommand(IBroker broker)
        {
            var command = new Command(this.Name, this.Description);
            command.Handler = this.Publisher(broker);

            foreach(string alias in this.Aliases)
            {
                command.AddAlias(alias);
            }

            foreach (Option option in this.Options)
            {
                command.AddOption(option);
            }

            foreach (Argument argument in this.Arguments)
            {
                command.AddArgument(argument);
            }

            return command;
        }
    }
}
