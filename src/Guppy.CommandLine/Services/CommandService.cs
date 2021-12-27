using Guppy.CommandLine.Interfaces;
using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Utilities;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;

namespace Guppy.CommandLine.Services
{
    public class CommandService : MessageQueue<ICommandData>
    {
        #region Private Fields
        private RootCommand _root;
        private IConsole _console;
        private Dictionary<Type, Command> _commands;
        #endregion

        #region Constructor
        public CommandService(Dictionary<Type, Command> commands)
        {
            _commands = commands;
            _root = new RootCommand()
            {
                Name = ">‎"
            };

            foreach(Command command in commands.Values.Where(command => command.Parents.Count == 0))
            {
                _root.AddCommand(command);
            }
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            provider.Service(out _console);
        }
        #endregion

        #region Methods
        public Command Get<TCommand>()
            where TCommand : class
                => _commands[typeof(TCommand)];

        public Command Get(Type type)
            => _commands[type];

        public Command Get(String input = "")
            => _root.Parse(input).CommandResult.Command as Command;

        public Int32 Invoke(String input = "")
            => _root.Invoke(input, _console);
        #endregion
    }
}
