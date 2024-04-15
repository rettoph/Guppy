using Guppy.Engine.Attributes;
using Guppy.Game.Commands.Extensions;
using Guppy.Game.Commands.TokenPropertySetters;
using Guppy.Core.Messaging;
using System.CommandLine;
using Guppy.Engine;

namespace Guppy.Game.Commands.Services
{
    [GuppyFilter<IGuppy>]
    internal sealed class CommandService : MagicBroker<ICommand>, ICommandService
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
            foreach (KeyValuePair<Command, SCL.Command> keyValuePair in _commands)
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
