using Guppy.Demos.Pong.Entities;
using Guppy.Demos.Pong.Layers;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Extensions;
using Microsoft.Xna.Framework;
using Guppy.Utilities.Cameras;

namespace Guppy.Demos.Pong.Scenes
{
    public sealed class GameScene : Scene
    {
        #region Private Fields
        private PrimitiveBatch _primitiveBatch;
        private Camera2D _camera;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _primitiveBatch = provider.GetService<PrimitiveBatch>();
            _camera = provider.GetService<Camera2D>();

            this.Layers.Create<MainLayer>(Int32.MinValue, Int32.MaxValue);

            this.Entities.Create<Ball>();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            _primitiveBatch.Begin(_camera.View, _camera.Projection);

            base.Draw(gameTime);

            _primitiveBatch.End();
        }
        #endregion
    }
}
