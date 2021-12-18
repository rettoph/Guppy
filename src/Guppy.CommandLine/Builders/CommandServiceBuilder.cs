using Guppy.CommandLine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.Builders
{
    public sealed class CommandServiceBuilder
    {
        private List<CommandBuilder> _commands = new List<CommandBuilder>();

        public CommandBuilder RegisterCommand(String name)
        {
            CommandBuilder builder = new CommandBuilder(name);
            _commands.Add(builder);

            return builder;
        }

        public CommandService Build()
        {
           return new CommandService(_commands.Select(cb => cb.Build()));
        }
    }
}
