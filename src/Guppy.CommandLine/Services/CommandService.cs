using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Text;

namespace Guppy.CommandLine.Services
{
    public class CommandService : Service
    {
        #region Private Fields
        private RootCommand _root;
        private IConsole _console;
        #endregion

        #region Constructor
        public CommandService(IEnumerable<Command> commands)
        {
            _root = new RootCommand()
            {
                Name = ">‎"
            };

            foreach(Command command in commands)
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
        public TCommand Get<TCommand>(String input = "")
            where TCommand : class, ICommand
                => _root.Parse(input).CommandResult.Command as TCommand;

        public Command Get(String input = "")
            => this.Get<Command>(input);

        public Int32 Invoke(String input = "")
            => _root.Invoke(input, _console);
        #endregion
    }
}
