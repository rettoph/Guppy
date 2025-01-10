using System.CommandLine;
using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Managers;
using Guppy.Core.Common;
using Guppy.Core.Messaging.Common.Implementations;

namespace Guppy.Core.Commands.Services
{
    public sealed class CommandService : Broker<ICommand>, ICommandService
    {
        private readonly RootCommand _root;
        private readonly Dictionary<Type, CommandManager> _managers;
        private readonly IConsole _console;

        public CommandService(
            IFiltered<ICommand> commands,
            ICommandTokenService tokenService,
            IConsole console)
        {
            this._console = console;
            this._managers = commands
                .Select(CommandContext.Create)
                .Select(x => CommandManager.Create(x, tokenService)).ToDictionary(x => x.Context.Type, x => x);
            this._root = new RootCommand()
            {
                Name = ">",
            };

            foreach (CommandManager manager in this._managers.Values)
            {
                manager.Initialize(this);
            }
        }

        public void AddCommand(Type? parent, Command command)
        {
            if (parent is null)
            {
                this._root.AddCommand(command);
            }
            else
            {
                this._managers[parent].Command.AddCommand(command);
            }
        }

        public void Invoke(string input) => this._root.Invoke(input, this._console);
    }
}