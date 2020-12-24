using Guppy.Lists;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using System.Data;
using System.Linq;
using Guppy.Interfaces;
using Guppy.IO.Commands.Delegates;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using Guppy.IO.Commands.Extensions;
using Guppy.Extensions.System;
using Guppy.Enums;

namespace Guppy.IO.Commands
{
    /// <summary>
    /// An instance of the data defined by a CommandContext
    /// object. This will manage sub-commands, parse incoming
    /// command strings, and build CommandArg instances.
    /// </summary>
    public abstract partial class CommandBase : Service, ICommandBase
    {
        #region Private Fields
        private Dictionary<String, ICommand> _commands;
        private ServiceProvider _provider;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public IReadOnlyDictionary<String, ICommand> Commands => _commands;

        /// <inheritdoc />
        public ICommand this[String word] => this.Commands[word];

        /// <inheritdoc />
        public abstract String Phrase { get; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _commands = new Dictionary<String, ICommand>();
        }

        protected override void Release()
        {
            base.Release();

            while (_commands.Any())
                this.TryRemove(_commands.First().Value);

            _commands = null;
        }
        #endregion

        #region Helper Methods
        /// <inheritdoc />
        public ICommand TryAddCommand(CommandContext context)
            => this.TryAddCommand(_provider.GetService<Command>((s, p, d) =>
                {
                    s.Word = context.Word;
                    s.PrimaryParent = this;
                    s.Description = context.Description;

                    context.Arguments?.ForEach(a => s.TryAddArgument(a));
                    context.Commands?.ForEach(ss => s.TryAddCommand(ss));
                }));

        /// <inheritdoc />
        public ICommand TryAddCommand(ICommand command)
        {
            if (this.Commands.ContainsKey(command.Word))
                throw new DuplicateNameException($"Unable to add Command '{command.Word}' into '{this.Phrase}'. Command alreayd exists.");

            _commands.Add(command.Word, command);
            command.OnStatus[ServiceStatus.Releasing] += this.HandleSubSegmentReleasing;

            return command;
        }


        public void TryRemove(String word)
            => _commands.Remove(word);

        public void TryRemove(Command segment)
        {
            if (_commands.Remove(segment.Word))
            {
                segment.OnStatus[ServiceStatus.Releasing] -= this.HandleSubSegmentReleasing;
                segment.TryRelease();
            }
        }

        public virtual String GetHelpText()
        {
            String help = "";

            if(this.Commands.Any())
            {
                help += "Available Commands:\n";
                this.Commands.Values.ForEach(c =>
                {
                    help += $"  {c.Word}{c.Description?.AddLeft(" - ")}\n";
                });
            }

            return help;
        }
        #endregion

        #region Event Handlers
        private void HandleSubSegmentReleasing(IService sender)
            => this.TryRemove(sender as Command);
        #endregion
    }
}
