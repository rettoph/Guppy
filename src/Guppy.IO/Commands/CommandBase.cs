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

namespace Guppy.IO.Commands
{
    /// <summary>
    /// An instance of the data defined by a CommandContext
    /// object. This will manage sub-commands, parse incoming
    /// command strings, and build CommandArg instances.
    /// </summary>
    public abstract partial class CommandBase : Service, ICommand
    {
        #region Private Fields
        private Dictionary<String, Command> _subCommands;
        private ServiceProvider _provider;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public abstract String Phrase { get; }

        /// <inheritdoc />
        public IReadOnlyDictionary<String, Command> SubCommands => _subCommands;

        /// <inheritdoc />
        public Command this[String word] => this.SubCommands[word];
        #endregion

        #region Events
        public event OnCommandExecuteDelegate OnExcecute;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _subCommands = new Dictionary<String, Command>();
        }

        protected override void Release()
        {
            base.Release();

            while (_subCommands.Any())
                this.TryRemove(_subCommands.First().Value);

            _subCommands = null;
        }
        #endregion

        #region Helper Methods

        public ICommand TryAddSubCommand(CommandContext context)
        {
            if (this.SubCommands.ContainsKey(context.Word))
                throw new DuplicateNameException($"Unable to create SubSegment '{context.Word}' into '{this.Phrase}'. SubSegment alreayd defined.");

            var sub = _provider.GetService<Command>((s, p, d) =>
            {
                s.Word = context.Word;
                s.Parent = this;

                context.Arguments?.ForEach(a => s.TryAddArgument(a));
                context.SubCommands?.ForEach(ss => s.TryAddSubCommand(ss));
            });

            _subCommands.Add(sub.Word, sub);
            sub.OnReleased += this.HandleSubSegmentReleased;

            return sub;
        }


        public void TryRemove(String word)
            => this.TryRemove(this.SubCommands[word]);

        public void TryRemove(Command segment)
        {
            if (_subCommands.Remove(segment.Word))
            {
                segment.OnReleased -= this.HandleSubSegmentReleased;
                segment.TryRelease();
            }
        }

        /// <summary>
        /// Parse the incoming command array and build
        /// a new CommandInstance based on the recieved
        /// data.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        internal abstract CommandArguments TryBuild(String[] input, Int32 position);

        /// <summary>
        /// Attempt to excecute a recieved command instance
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal virtual void Execute(CommandArguments arguments)
        {
            this.OnExcecute?.Invoke(this, arguments);
        }
        #endregion

        #region Event Handlers
        private void HandleSubSegmentReleased(IService sender)
            => this.TryRemove(sender as Command);
        #endregion
    }
}
