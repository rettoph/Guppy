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
        private IReadOnlyDictionary<ButtonState, CommandArguments> _commandArguments;
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
        /// The command that should be parsed
        /// & executed on input.
        /// </summary>
        public String Command { get; private set; }

        /// <summary>
        /// A list of all input states that can invoke the
        /// current InputCommand.
        /// </summary>
        public ButtonState[] States { get; private set; }
        #endregion

        #region Constructor
        internal InputCommand()
        {
        }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _commands);
            provider.Service(out _inputs);
            this.ConfigureInput(this.Input);

            _commandArguments = this.States.ToDictionary(
                keySelector: bs => bs,
                elementSelector: bs => _commands.TryBuild(this.ParseCommand(bs)).Copy());
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
            if (this.InitializationStatus >= InitializationStatus.Initializing)
                throw new Exception("Unable to set context after initialization has begin");

            this.Handle = context.Handle;
            this.Input = context.DefaultInput;
            this.Command = context.Command;
            this.States = context.States;
        }

        /// <summary>
        /// Return the command string parsed as if the recieved button
        /// state event just took place.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="bs"></param>
        /// <returns></returns>
        private String ParseCommand(ButtonState bs)
            => this.Command.Replace(InputCommand.StateShortcode, bs == ButtonState.Pressed ? "true" : "false");

        /// <summary>
        /// Update the input listeners to the specified input type.
        /// </summary>
        /// <param name="input"></param>
        public void ConfigureInput(InputType? input)
        {
            if(_inputManager != null)
            { // Unset the old manager...
                this.States.ForEach(bs => _inputManager.OnState[bs] -= this.HandleInput);
            }

            if (input != null)
            { // Configure the new value...
                this.Input = input.Value;
                _inputManager = _inputs.GetManager(this.Input);
                this.States.ForEach(bs => _inputManager.OnState[bs] += this.HandleInput);
            }
        }
        #endregion

        #region Event Handlers
        private void HandleInput(InputManager sender, InputArgs args)
            => _commands.TryExecute(_commandArguments[args.State]);
        #endregion
    }
}
