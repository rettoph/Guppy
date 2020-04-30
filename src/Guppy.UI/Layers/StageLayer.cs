using Guppy.DependencyInjection;
using Guppy.Extensions.Utilities;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Layers
{
    /// <summary>
    /// A simple layer built to handle UI elements. This is not required,
    /// but may be the easiest route to setup quick UIs.
    /// </summary>
    public class StageLayer : Layer
    {
        #region Private Fields
        private PrimitiveBatch _primitiveBatch;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _camera);
            provider.Service(out _primitiveBatch);
            provider.Service(out _spriteBatch);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _camera.Center = false;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            _primitiveBatch.Begin(_camera);
            _spriteBatch.Begin();

            base.Draw(gameTime);

            _spriteBatch.End();
            _primitiveBatch.End();
        }
        #endregion
    }
}
