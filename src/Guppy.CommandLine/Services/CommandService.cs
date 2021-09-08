using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.System;
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

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            provider.Service(out _root);
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
