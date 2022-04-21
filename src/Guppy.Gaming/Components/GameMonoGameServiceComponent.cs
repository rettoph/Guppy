using Guppy.EntityComponent;
using Guppy.Gaming.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Components
{
    internal sealed class GameMonoGameServiceComponent : IComponent
    {
        private Game _game;
        private ITerminalService _terminal;
        private IInputService _inputs;

        public GameMonoGameServiceComponent(Game game, ITerminalService terminal, IInputService inputs)
        {
            _game = game;
            _terminal = terminal;
            _inputs = inputs;

            _game.OnUpdate += this.Update;
            _game.OnDraw += this.Draw;
        }

        public void Dispose()
        {
            _game.OnUpdate -= this.Update;
            _game.OnDraw -= this.Draw;
        }

        private void Draw(GameTime gameTime)
        {
            _terminal.Draw(gameTime);
        }

        private void Update(GameTime gameTime)
        {
            _inputs.Update(gameTime);
            _terminal.Update(gameTime);
        }
    }
}
