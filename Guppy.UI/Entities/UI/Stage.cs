using Guppy.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    public class Stage : Element
    {
        #region Private Fields
        private GameWindow _window;
        private Rectangle _viewport;
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _window = provider.GetRequiredService<GameWindow>();
            _viewport = new Rectangle(0, 0, _window.ClientBounds.Width - 1, _window.ClientBounds.Height - 1);
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.SetEnabled(true);
            this.SetVisible(true);

            _window.ClientSizeChanged += this.HandleClientSizeChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientSizeChanged;
        }
        #endregion

        #region Methods
        protected internal override Rectangle GetParentBounds()
        {
            return _viewport;
        }
        #endregion

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            this.dirty = true;
            _viewport = new Rectangle(0, 0, _window.ClientBounds.Width - 1, _window.ClientBounds.Height - 1);
        }
        #endregion
    }
}
