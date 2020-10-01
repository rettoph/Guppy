using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Commands;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using Guppy.IO.Input.Services;
using Guppy.IO.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Services
{
    /// <summary>
    /// Simple class to track & manage UI related values.
    /// </summary>
    public sealed class UIService : Service
    {
        #region Private Fields
        private MouseService _mouse;
        private CommandService _commands;
        private Boolean _pressed;
        #endregion

        #region Public Properties
        /// <summary>
        /// The current pixel representation of the
        /// user's target. By default this is pulled
        /// from the mouse position but may be changed
        /// in the future due to touch screen support.
        /// </summary>
        public Vector2 Target => _mouse.Position;

        /// <summary>
        /// The current pressed state of the UI.
        /// This will reflect changes when a user
        /// "click" or "taps" on the UI elements.
        /// </summary>
        public Boolean Pressed
        {
            get => _pressed;
            set
            {
                if(value != _pressed)
                {
                    _pressed = value;
                    this.OnPressedChanged?.Invoke(this.Pressed);
                }
            }
        }
        #endregion

        #region Events
        public event GuppyEventHandler<Boolean> OnPressedChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _mouse);
            provider.Service(out _commands);

            _commands["ui"]["interact"].OnExcecute += this.HandleUIInteractCommand;
        }
        #endregion

        #region Event Handlers
        private void HandleUIInteractCommand(ICommand sender, CommandArguments args)
            => this.Pressed = (ButtonState)args["state"] == ButtonState.Pressed;
        #endregion
    }
}
