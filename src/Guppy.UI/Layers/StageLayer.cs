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
        private RasterizerState _rasterizerState;
        private GraphicsDevice _graphics;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _graphics);
            provider.Service(out _camera);
            provider.Service(out _primitiveBatch);
            provider.Service(out _spriteBatch);

            _rasterizerState = new RasterizerState() {
                ScissorTestEnable = true,
                MultiSampleAntiAlias = true
            };
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
            _graphics.ScissorRectangle = _graphics.Viewport.Bounds;
            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointWrap, rasterizerState: _rasterizerState);
            _primitiveBatch.Begin(_camera, BlendState.AlphaBlend);

            base.Draw(gameTime);
            
            _primitiveBatch.End();
            _spriteBatch.End();
        }
        #endregion
    }
}
