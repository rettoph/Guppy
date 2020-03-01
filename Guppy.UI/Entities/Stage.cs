using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Collections;
using Guppy.UI.Components;
using Guppy.UI.Components.Interfaces;
using Guppy.UI.Entities;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    public class Stage : Container<Element>, IEntity
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        private GameWindow _window;
        #endregion

        #region Lifecycle Method 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _graphics = provider.GetRequiredService<GraphicsDevice>();
            _window = provider.GetRequiredService<GameWindow>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.Bounds.Set(0, 0, 1f, 1f);
            this.Container = new StageContainer(_graphics);

            this.Enabled = true;
            this.Visible = true;
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            _window.ClientSizeChanged += this.HandleClientSizeChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientSizeChanged;
        }
        #endregion

        #region Helper Methods
        public override Rectangle GetBounds()
        {
            return _graphics.Viewport.Bounds;
        }
        #endregion

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            this.Dirty = true;
        }
        #endregion

        #region IEntity Implementation
        #region Private Fields
        private Int32 _layerDepth;
        #endregion

        #region
        public Int32 LayerDepth
        {
            get => _layerDepth;
            set
            {
                if (_layerDepth != value)
                {
                    _layerDepth = value;

                    this.OnLayerDepthChanged?.Invoke(this, this.LayerDepth);
                }
            }
        }
        #endregion

        #region Events & Delegaters 
        public event EventHandler<Int32> OnLayerDepthChanged;
        #endregion
        #endregion
    }

    sealed class StageContainer : IBaseElement
    {
        private GraphicsDevice _graphics;

        public bool Dirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Hovered => true;

        public StageContainer(GraphicsDevice graphics)
        {
            this._graphics = graphics;
        }

        public Rectangle GetBounds()
        {
            return _graphics.Viewport.Bounds;
        }

        public void TryClean(bool force = false)
        {
            throw new NotImplementedException();
        }
    }

}
