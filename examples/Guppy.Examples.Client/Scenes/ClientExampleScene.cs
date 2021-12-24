using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Scenes;
using Guppy.Extensions.Utilities;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Examples.Client.Scenes
{
    public sealed class ClientExampleScene : ExampleScene
    {
        #region Private Fields
        private PrimitiveBatch<VertexPositionColor> _primitiveBatch;
        private Camera2D _camera;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _primitiveBatch);
            provider.Service(out _camera);

            _camera.Position = Vector2.Zero;
            _camera.Center = false;
        }
        #endregion

        #region Frame Methods
        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            _camera.TryClean(gameTime);
            _primitiveBatch.Begin(_camera);
        }

        protected override void PostDraw(GameTime gameTime)
        {
            base.PostDraw(gameTime);

            _primitiveBatch.End();
        }
        #endregion
    }
}
