using Guppy.MonoGame;
using Guppy.MonoGame.Services;
using Guppy.Resources.Constants;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library
{
    public class ExampleGuppy : FrameableGuppy
    {
        private ITerminalService _terminal;
        private IInputService _inputs;


        public ExampleGuppy(ITerminalService terminal, IInputService inputs, World world) : base(world)
        {
            _terminal = terminal;
            _inputs = inputs;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _terminal.Update(gameTime);
            _inputs.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _terminal.Draw(gameTime);
        }
    }
}
