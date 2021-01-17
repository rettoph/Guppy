using Guppy.DependencyInjection;
using Guppy.UI.Elements;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Enums;
using Guppy.Utilities.Primitives;

namespace Guppy.UI.Entities
{
    /// <summary>
    /// A simple entity designed to contain
    /// & manage elements.
    /// </summary>
    public class Stage : Entity, IContainer
    {
        #region Private Fields
        private GameWindow _window;
        private GraphicsDevice _graphics;
        #endregion

        #region Public Properties
        /// <summary>
        /// The primary root level element container within the stage.
        /// </summary>
        public PageContainer Content { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _window);
            provider.Service(out _graphics);

            this.Content = provider.GetService<PageContainer>((content, p, d) =>
            {
                content.Bounds.Width = 1f;
                content.Bounds.Height = 1f;
                content.Container = this;
            });

            this.Content.TryCleanBounds();

            _window.ClientSizeChanged += this.HandleClientSizeChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Content.TryCleanHovered();
            this.Content.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.Content.TryDraw(gameTime);
        }
        #endregion

        #region IContainer Implementation
        Rectangle IContainer.GetInnerBoundsForChildren()
            => new Rectangle(
                1,
                0,
                _graphics.Viewport.Bounds.Width - 2,
                _graphics.Viewport.Bounds.Height - 1);
        #endregion

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
            => this.Content.TryCleanBounds();
        #endregion
    }
}
