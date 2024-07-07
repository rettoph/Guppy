using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Common.TokenPropertySetters;
using Guppy.Core.Messaging.Common.Implementations;
using System.CommandLine;

namespace Guppy.Core.Commands.Services
{
    public sealed class CommandService : Broker<ICommand>, ICommandService
    {
        private readonly RootCommand _root;
        private Dictionary<Common.Command, SCL.Command> _commands;
        private IConsole _console;

        public CommandService(
            IEnumerable<Common.Command> commands,
            IEnumerable<ITokenPropertySetter> tokenSetters,
            IConsole console)
        {
            _console = console;
            _commands = new Dictionary<Common.Command, SCL.Command>();
            _root = new RootCommand()
            {
                Name = ">",
            };

            var tokenSettersArray = tokenSetters.ToArray();
            foreach (Common.Command command in commands)
            {
                _commands.Add(command, command.GetSystemCommand(this, tokenSettersArray));
            }

            // Nest all commands as needed
            foreach (KeyValuePair<Common.Command, SCL.Command> keyValuePair in _commands)
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
