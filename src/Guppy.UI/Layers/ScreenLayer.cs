using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
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
    /// Simple layer that will render 1 to 1 pixel screen
    /// visuals (such as the base UI)
    /// </summary>
    public class ScreenLayer : Layer
    {
        #region Private Fields
        private PrimitiveBatch<VertexPositionColor> _primitiveBatch;
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

            _rasterizerState = new RasterizerState()
            {
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
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _camera.TryClean(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.ScissorRectangle = _graphics.Viewport.Bounds;
            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointWrap, rasterizerState: _rasterizerState);
            _primitiveBatch.Begin(_camera.View, _camera.Projection, BlendState.AlphaBlend);

            base.Draw(gameTime);

            _primitiveBatch.End();
            _spriteBatch.End();
        }
        #endregion
    }
}
