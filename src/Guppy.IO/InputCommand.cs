﻿using Guppy.CommandLine.Services;
using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.IO.Contexts;
using Guppy.IO.Services;
using Guppy.IO.Structs;
using Guppy.IO.Utilities;
using Guppy.Threading.Utilities;
using Guppy.Utilities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        #region Static Properties
        public static String StateShortcode { get; private set; } = "[state]";
        #endregion

        #region Private Fields
        private CommandService _commands;
        private InputButtonService _inputs;
        private InputButtonManager _inputManager;
        private ThreadQueue _threadQueue;
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
        public IReadOnlyDictionary<ButtonState, String> Commands { get; private set; }

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
            provider.Service(Constants.ServiceNames.GameUpdateThreadQueue, out _threadQueue);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.ConfigureInput(this.Input);
        }

        protected override void Release()
        {
            base.Release();

            _commands = null;
            _inputs = null;
            _threadQueue = null;

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
            this.Commands = context.Commands.ToDictionary(
                keySelector: bsc => bsc.state,
                elementSelector: bsc => bsc.command);
            this.Lockable = context.Lockable;
        }

        /// <summary>
        /// Update the input listeners to the specified input type.
        /// </summary>
        /// <param name="input"></param>
        public void ConfigureInput(InputButton? input)
        {
            if (_inputManager != null)
            { // Unset the old manager...
                this.Commands.Keys.ForEach(bs => _inputManager.OnState[bs] -= this.HandleInput);
            }

            if (input != null)
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

            _threadQueue.Enqueue(gt => _commands.Invoke(this.Commands[args.State]));
        }
        #endregion
    }
}
