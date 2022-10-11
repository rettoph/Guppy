using Guppy.MonoGame.Services;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Systems
{
    internal sealed class TerminalSystem : IUpdateSystem, IDrawSystem
    {
        private ITerminalService _terminal;

        public TerminalSystem(ITerminalService terminal)
        {
            _terminal = terminal;
        }

        public void Initialize(World world)
        {
            // throw new NotImplementedException();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            _terminal.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _terminal.Draw(gameTime);
        }
    }
}
