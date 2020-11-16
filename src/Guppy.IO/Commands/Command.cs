using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Delegates;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Guppy.IO.Commands
{
    public class Command : CommandBase, ICommand
    {
        #region Private Fields
        private List<ArgContext> _args;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public ICommandBase PrimaryParent { get; internal set; }

        /// <inheritdoc />
        public override String Phrase => (this.PrimaryParent.Phrase + ' ' + this.Word).TrimStart();

        /// <inheritdoc />
        public String Word { get; internal set; }

        /// <inheritdoc />
        public IReadOnlyCollection<ArgContext> Arguments => _args;

        /// <inheritdoc />
        public String Description { get; internal set; }
        #endregion

        #region Events
        public event OnCommandExecuteDelegate OnExcecute;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _args = new List<ArgContext>();
        }

        protected override void Release()
        {
            this.PrimaryParent = null;
        }

        public override string GetHelpText()
        {
            String help = this.Description?.AddRight('\n') + base.GetHelpText();

            if (this.Arguments.Any())
            {
                help += "\nArguments:\n";
                this.Arguments.ForEach(a =>
                {
                    help += $"  {a.Identifier} {(a.Required ? "(Required)" : "(Optional)")}{a.Description?.AddLeft(" - ")}\n";

                    if(a.Aliases != default)
                        help += $"    Aliases: {String.Join(", ", a.Aliases)}\n";

                    help += $"    Type: {a.Type}\n";
                });
            }

            return help.TrimStart('\n');
        }
        #endregion

        #region ICommand Implementation
        IEnumerable<CommandResponse> ICommand.LazyExcecute(CommandInput input)
            => this.OnExcecute.LazyInvoke(this, input);
        #endregion

        #region Helper Methods
        public void TryAddArgument(ArgContext arg)
            => _args.Add(arg);
        public void TryRemoveArgument(ArgContext arg)
            => _args.Remove(arg);

        public CommandInput BuildInput(String[] input, Int32 position)
            => new CommandInput(this, input, position);
        #endregion
    }
}
