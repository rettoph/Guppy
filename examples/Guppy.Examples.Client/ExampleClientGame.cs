using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Example.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guppy.Examples.Client
{
    public class ExampleClientGame : ExampleGame
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _graphics);
        }
        #endregion

        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            _graphics.Clear(Color.Black);
        }
    }
}
