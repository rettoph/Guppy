using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Managers;
using Guppy.Core.Common;
using Guppy.Core.Messaging.Common.Implementations;
using System.CommandLine;

namespace Guppy.Core.Commands.Services
{
    public sealed class CommandService : Broker<ICommand>, ICommandService
    {
        private readonly RootCommand _root;
        private Dictionary<Type, CommandManager> _managers;
        private IConsole _console;

        public CommandService(
            IFiltered<ICommandContext> commandContexts,
            ICommandTokenService tokenService,
            IConsole console)
        {
            _console = console;
            _managers = commandContexts.Select(x => CommandManager.Create(x, tokenService)).ToDictionary(x => x.Context.Type, x => x);
            _root = new RootCommand()
            {
                Name = ">",
            };

            foreach (CommandManager manager in _managers.Values)
            {
                manager.Initialize(this);
            }
        }

        public void AddCommand(Type? parent, Command command)
        {
            if (parent is null)
            {
                _root.AddCommand(command);
            }
            else
            {
                _managers[parent].Command.AddCommand(command);
            }
        }

        public void Invoke(string input)
        {
            _root.Invoke(input, _console);
        }
    }
}
