using Guppy.Attributes;
using Guppy.Commands.Extensions;
using Guppy.Commands.TokenPropertySetters;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using Guppy.Messaging;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Guppy.Commands.Services
{
    [GuppyFilter<IGuppy>]
    internal sealed class CommandService : Broker<ICommand>, ICommandService
    {
        private readonly RootCommand _root;
        private Dictionary<Command, SCL.Command> _commands;
        private IConsole _console;

        public CommandService(
            IEnumerable<Command> commands, 
            IEnumerable<ITokenPropertySetter> tokenSetters,
            IConsole console)
        {
            _console = console;
            _commands = new Dictionary<Command, SCL.Command>();
            _root = new RootCommand()
            {
                Name = ">",
            };

            var tokenSettersArray = tokenSetters.ToArray();
            foreach (Command command in commands)
            {
                _commands.Add(command, command.GetSystemCommand(this, tokenSettersArray));
            }

            // Nest all commands as needed
            foreach(KeyValuePair<Command, SCL.Command> keyValuePair in _commands)
            {
                SCL.Command parent = keyValuePair.Key.Parent is null ? _root : _commands.First(x => x.Key.Type == keyValuePair.Key.Parent).Value;
                parent.AddCommand(keyValuePair.Value);
            }
        }

        public void Invoke(string input)
        {
            _root.Invoke(input, _console);
        }
    }
}
