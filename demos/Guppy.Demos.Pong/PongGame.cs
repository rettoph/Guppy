using Guppy.Attributes;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Extensions;

namespace Guppy.Demos.Pong
{
    public class PongGame : Guppy.Game
    {
        private GraphicsDevice _graphics;

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _graphics = provider.GetService<GraphicsDevice>();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _graphics.Clear(Color.Brown);
        }
    }
}
