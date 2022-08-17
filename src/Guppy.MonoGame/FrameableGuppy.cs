using Guppy.MonoGame.Services;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public abstract class FrameableGuppy : IGuppy
    {
        private readonly ITerminalService _terminal;
        private readonly IDebuggerService _debugger;

        private World _world;

        protected World world => _world;

        public FrameableGuppy(ITerminalService terminal, IDebuggerService debugger, World world)
        {
            _terminal = terminal;
            _debugger = debugger;
            _world = world;
        }

        public virtual void Update(GameTime gameTime)
        {
            _terminal.Update(gameTime);
            _debugger.Update(gameTime);

            _world.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            _terminal.Draw(gameTime);
            _debugger.Draw(gameTime);

            _world.Draw(gameTime);
        }
    }
}
