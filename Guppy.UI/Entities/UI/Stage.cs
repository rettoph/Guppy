using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities.UI
{
    public class Stage : Element
    {
        #region Private Fields
        private SpriteBatch _spriteBatch;
        private PrimitiveBatch _primitiveBatch;
        private Pointer _pointer;
        private GraphicsDevice _graphics;
        private GameWindow _window;
        #endregion

        #region Protected Fields
        protected override SpriteBatch spriteBatch => _spriteBatch;

        protected override PrimitiveBatch primitiveBatch => _primitiveBatch;

        protected override Pointer pointer => _pointer;
        #endregion

        #region Lifecycle Method 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _spriteBatch = provider.GetRequiredService<SpriteBatch>();
            _primitiveBatch = provider.GetRequiredService<PrimitiveBatch>();
            _pointer = provider.GetRequiredService<Pointer>();
            _graphics = provider.GetRequiredService<GraphicsDevice>();
            _window = provider.GetRequiredService<GameWindow>();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.Bounds.Set(0, 0, Unit.Get(1f, -1), Unit.Get(1f, -1));

            _window.ClientSizeChanged += this.HandleClientSizeChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientSizeChanged;
        }
        #endregion

        #region Helper Methods
        public override Rectangle GetContainerBounds()
        {
            return _graphics.Viewport.Bounds;
        }
        #endregion

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            this.dirty = true;
        }
        #endregion
    }
}
