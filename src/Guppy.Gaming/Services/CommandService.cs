using Guppy.Gaming.Definitions;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    internal sealed class CommandService : Broker, ICommandService
    {
        private RootCommand _root;

        public CommandService(IEnumerable<CommandDefinition> definitions)
        {
            _root = new RootCommand();

            // Build all defined commands...
            var commands = definitions.ToDictionary(x => x, x => x.BuildCommand(this));
            // Add all subcommands into the new commands...
            foreach (var command in commands)
            {
                foreach (var subCommand in commands.Where(x => x.Key.Parent == command.Key.GetType()))
                {
                    command.Value.AddCommand(subCommand.Value);
                }
            }

            foreach(var command in commands.Where(x => x.Key.Parent is null))
            {
                _root.AddCommand(command.Value);
            }
        }

        public void Invoke(string command)
        {
            _root.Invoke(command);
        }
    }
}
