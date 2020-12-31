using Guppy.DependencyInjection;
using Guppy.IO.Commands;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using Guppy.IO.Output.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Output.Drivers.Scenes
{
    /// <summary>
    /// A simple driver that will implement a basic terminal 
    /// overlay ontop of game insances.
    /// </summary>
    internal sealed class GameTerminalDriver : Driver<Game>
    {
        #region Private Fields
        private Boolean _visible;
        private TerminalService _terminal;
        private CommandService _commands;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(Game driven, ServiceProvider provider)
        {
            base.Initialize(driven, provider);

            provider.Service(out _terminal);
            provider.Service(out _commands);

            _commands["terminal"].OnExcecute += this.HandleTerminalCommand;
        }

        protected override void Release(Game driven)
        {
            base.Release(driven);

            // Remove events
            _commands["terminal"].OnExcecute -= this.HandleTerminalCommand;
            this.driven.OnPostUpdate -= _terminal.TryUpdate;
            this.driven.OnPostDraw -= _terminal.TryDraw;
             
            // Clear references.
            _terminal = null;
            _commands = null;
        }
        #endregion

        #region Event Handlers
        private CommandResponse HandleTerminalCommand(ICommand sender, CommandInput input)
        {
            if(_visible)
            { // Stop rendering the terminal
                this.driven.OnPostUpdate -= _terminal.TryUpdate;
                this.driven.OnPostDraw -= _terminal.TryDraw;
                _terminal.Close();
            }
            else
            { // Begin rendering the terminal
                this.driven.OnPostUpdate += _terminal.TryUpdate;
                this.driven.OnPostDraw += _terminal.TryDraw;
                _terminal.Open();
            }

            _visible = !_visible;

            return CommandResponse.Empty;
        }
        #endregion
    }
}
