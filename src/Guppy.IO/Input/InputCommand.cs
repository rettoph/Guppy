using Guppy.IO.Commands.Services;
using Guppy.IO.Input.Contexts;
using Guppy.IO.Input.Services;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Enums;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.IO.Commands;
using System.Linq;
using Guppy.Extensions.Collections;
using Guppy.Utilities;

namespace Guppy.IO.Input
{
    /// <summary>
    /// Represents a link between an input event
    /// & a command execution. To create a new
    /// input command add it to the 
    /// InputCommandService.
    /// </summary>
    public class InputCommand : Service
    {
        #region Static Properties
        public static String StateShortcode { get; private set; } = "[state]";
        #endregion

        #region Private Fields
        private CommandService _commands;
        private InputService _inputs;
        private InputManager _inputManager;
        private Synchronizer _synchronizer;
        #endregion

        #region Public Properties
        /// <summary>
        /// A unique human readable name for this
        /// specific InputCommand.
        /// </summary>
        public String Handle { get; private set; }

        /// <summary>
        /// The input type that will trigger a command invocation.
        /// </summary>
        public InputType Input { get; private set; }

        /// <summary>
        /// An dictionary of button states and the command to run
        /// </summary>
        public IReadOnlyDictionary<ButtonState, CommandInput> CommandArguments { get; private set; }
        #endregion

        #region Constructor
        internal InputCommand()
        {
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _commands);
            provider.Service(out _inputs);
            provider.Service(out _synchronizer);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.ConfigureInput(this.Input);
        }

        protected override void Release()
        {
            base.Release();

            this.ConfigureInput(null);
        }
        #endregion

        #region Helper Methods
        internal void SetContext(InputCommandContext context)
        {
            if (this.Status >= ServiceStatus.Initializing)
                throw new Exception("Unable to set context after initialization has begin");

            this.Handle = context.Handle;
            this.Input = context.DefaultInput;
            this.CommandArguments = context.Commands.ToDictionary(
                keySelector: bsc => bsc.state,
                elementSelector: bsc => _commands.TryBuild(bsc.command).Copy());
        }

        /// <summary>
        /// Update the input listeners to the specified input type.
        /// </summary>
        /// <param name="input"></param>
        public void ConfigureInput(InputType? input)
        {
            if(_inputManager != null)
            { // Unset the old manager...
                this.CommandArguments.Keys.ForEach(bs => _inputManager.OnState[bs] -= this.HandleInput);
            }

            if (input != null)
            { // Configure the new value...
                this.Input = input.Value;
                _inputManager = _inputs.GetManager(this.Input);
                this.CommandArguments.Keys.ForEach(bs => _inputManager.OnState[bs] += this.HandleInput);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Execute the command within a synchronized call.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void HandleInput(InputManager sender, InputArgs args)
            => _synchronizer.Enqueue(gt => _commands.TryExecute(this.CommandArguments[args.State]));
        #endregion
    }
}
