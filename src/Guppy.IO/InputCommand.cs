using Guppy.CommandLine.Services;
using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.IO.Contexts;
using Guppy.IO.Services;
using Guppy.IO.EventArgs;
using Guppy.IO.Utilities;
using Guppy.Threading.Utilities;
using Guppy.Utilities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.IO.Structs;
using Guppy.CommandLine.Interfaces;

namespace Guppy.IO
{
    /// <summary>
    /// Represents a link between an input event
    /// & a command execution. To create a new
    /// input command add it to the 
    /// InputCommandService.
    /// </summary>
    public class InputCommand : Service
    {
        #region Private Fields
        private CommandService _commands;
        private InputButtonService _inputs;
        private InputButtonManager _inputManager;
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
        public InputButton Input { get; private set; }

        /// <summary>
        /// An dictionary of button states and the command to run
        /// </summary>
        public IReadOnlyDictionary<ButtonState, ICommandData> Commands { get; private set; }

        /// <summary>
        /// The owning <see cref="InputCommandService"/>. This will
        /// automatically be defined upon construction.
        /// </summary>
        public InputCommandService InputCommandService { get; internal set; }

        /// <summary>
        /// When true, then the command input will 
        /// not excecute if <see cref="InputCommandService.Locked"/>
        /// is true. This generally only happens when the terminal 
        /// is opened.
        /// </summary>
        public Boolean Lockable { get; set; }
        #endregion

        #region Constructor
        public InputCommand()
        {
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _commands);
            provider.Service(out _inputs);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.ConfigureInput(this.Input);
        }

        protected override void Uninitialize()
        {
            base.Uninitialize();

            this.ConfigureInput(null);
        }
        #endregion

        #region Helper Methods
        internal void SetContext(
            String handle, 
            InputButton input, 
            Dictionary<ButtonState, ICommandData> commands,
            Boolean lockable)
        {
            this.Handle = handle;
            this.Input = input;
            this.Commands = commands;
            this.Lockable = lockable;
        }

        /// <summary>
        /// Update the input listeners to the specified input type.
        /// </summary>
        /// <param name="input"></param>
        public void ConfigureInput(InputButton? input)
        {
            if (_inputManager is not null)
            { // Unset the old manager...
                this.Commands.Keys.ForEach(bs => _inputManager.OnState[bs] -= this.HandleInput);
            }

            if (input is not null)
            { // Configure the new value...
                this.Input = input.Value;
                _inputManager = _inputs.GetManager(this.Input);
                this.Commands.Keys.ForEach(bs => _inputManager.OnState[bs] += this.HandleInput);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Execute the command within a synchronized call.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void HandleInput(InputButtonManager sender, InputButtonArgs args)
        {
            if (this.Lockable && this.InputCommandService.Locked)
                return;

            _commands.Publish(this.Commands[args.State]);
        }
        #endregion
    }
}
