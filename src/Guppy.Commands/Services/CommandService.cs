using Guppy.Commands.Extensions;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Guppy.Commands.Services
{
    internal sealed class CommandService : ICommandService
    {
        private readonly RootCommand _root;
        private Dictionary<Command, SCL.Command> _commands;

        public CommandService(IBus bus, IEnumerable<Command> commands)
        {
            _commands = new Dictionary<Command, SCL.Command>();
            _root = new RootCommand()
            {
                Name = ">"
            };

            foreach (Command command in commands)
            {
                _commands.Add(command, command.GetSystemCommand(bus));
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
            _root.Invoke(input);
        }
    }
}
