using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Simple scrollable element
    /// </summary>
    public class ScrollableList : SimpleElement
    {
        public ScrollableListItemContainer Items { get; private set; }
        public ScrollBar ScrollBarContainer { get; private set; }

        private RenderTargetBinding[] _cachedRenderTargets;
        private RenderTarget2D _contentRenderTarget;

        public ScrollableList(Unit x, Unit y, Unit width, Unit height, StyleSheet rootStyleSheet = null) : base(x, y, width, height, rootStyleSheet)
        {
            this.Items = new ScrollableListItemContainer(this);
            this.ScrollBarContainer = new ScrollBar(this, rootStyleSheet);
        }

        #region Frame Methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // First draw the scrollbar
            this.ScrollBarContainer.Draw(gameTime, spriteBatch);

            // Next draw the content on an isolated render target
            _cachedRenderTargets = this.graphicsDevice.GetRenderTargets();
            this.graphicsDevice.SetRenderTarget(_contentRenderTarget);

            this.graphicsDevice.Clear(Color.Transparent);

            this.internalSpriteBatch.Begin();
            this.Items.Draw(gameTime, this.internalSpriteBatch);
            this.internalSpriteBatch.End();

            // Revert back to the original render targets...
            this.graphicsDevice.SetRenderTargets(_cachedRenderTargets);

            // Draw the content within the bounds of the frame
            spriteBatch.Draw(_contentRenderTarget, this.Bounds, this.Bounds, Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            if (this.ScrollBarContainer.Handle.State == ElementState.Pressed && this.inputManager.Mouse.Delta.Y != 0)
            {
                var deltaPercent = this.inputManager.Mouse.Delta.Y / (this.ScrollBarContainer.Height - this.ScrollBarContainer.Handle.Height);
                this.ScrollBarContainer.Scroll(deltaPercent);
            }
            if(this.State == ElementState.Hovered && this.inputManager.Mouse.ScrollDelta != 0)
            {
                var deltaPercent = ((Single)this.inputManager.Mouse.ScrollDelta / -12) / (this.ScrollBarContainer.Height - this.ScrollBarContainer.Handle.Height);
                this.ScrollBarContainer.Scroll(deltaPercent);
            }

            base.Update(gameTime);

            this.ScrollBarContainer.Update(gameTime);
            this.Items.Update(gameTime);
        }
        #endregion

        protected internal override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            base.AddDebugVertices(ref vertices);

            this.ScrollBarContainer.AddDebugVertices(ref vertices);
            this.Items.AddDebugVertices(ref vertices);
        }
        protected internal override void UpdateCache()
        {
            base.UpdateCache();

            _contentRenderTarget?.Dispose();
            _contentRenderTarget = new RenderTarget2D(this.graphicsDevice, this.window.ClientBounds.Width, this.window.ClientBounds.Height);

            this.Items.UpdateCache();
            this.ScrollBarContainer.UpdateCache();
        }
        protected internal override void UpdateBounds()
        {
            base.UpdateBounds();

            if (this.Items.Height > this.Height)
                this.Items.Y = (Int32)((this.Height - this.Items.Height) * this.ScrollBarContainer.Value);
            else
                this.Items.Y = 0;

            this.Items.UpdateBounds();

            this.ScrollBarContainer.UpdateBounds();
        }
    }
}
