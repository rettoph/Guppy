using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.IO.Args;
using Guppy.IO.Services;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Keyboard = Guppy.IO.Services.Keyboard;

namespace Guppy.IO.Components
{
    internal sealed class GameTerminalComponent : Component<Game>
    {
        #region Private Fields
        private Game _game;
        private Terminal _terminal;
        private Keyboard _keyboard;
        private Boolean _terminalState;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _game);
            provider.Service(out _terminal);
            provider.Service(out _keyboard);

            _keyboard[Keys.OemTilde].OnState[ButtonState.Pressed] += this.HandleToggleTerminal;
        }

        protected override void Release()
        {
            base.Release();

            _keyboard[Keys.OemTilde].OnState[ButtonState.Pressed] -= this.HandleToggleTerminal;
        }
        #endregion

        #region Events
        private void HandleToggleTerminal(InputButtonManager sender, InputButtonArgs args)
        {
            _terminalState = !_terminalState;

            if(_terminalState)
            {
                _terminal.Open(_game);
            }
            else
            {
                _terminal.Close(_game);
            }
        }
        #endregion
    }
}
