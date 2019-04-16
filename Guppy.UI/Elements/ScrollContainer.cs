using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Guppy.UI.Enums;

namespace Guppy.UI.Elements
{
    public class ScrollContainer : Element
    {
        #region Private Fields
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _scrollContainer;
        private BasicEffect _internalEffect;
        private GraphicsDevice _graphicsDevice;
        #endregion

        #region Public Fields
        public Single Scroll { get; private set; }
        public Container Items { get; private set; }
        public Scrollbar ScrollBar { get; private set; }
        #endregion

        #region Events
        public event EventHandler<ScrollContainer> OnScrolled;
        #endregion

        public ScrollContainer(
            Unit x,
            Unit y,
            Unit width,
            Unit height,
            Style style = null) : base(x, y, width, height, style)
        {
            this.Items = this.add(new ScrollItems(this)) as Container;
            this.ScrollBar = this.add(new Scrollbar(this)) as Scrollbar;

            this.StateBlacklist = ElementState.Active | ElementState.Pressed | ElementState.Hovered;
        }

        public override void CleanTexture(GraphicsDevice graphicsDevice, RenderTarget2D layerRenderTarget, RenderTarget2D outputRenderTarget, SpriteBatch spriteBatch)
        {
            _graphicsDevice = graphicsDevice;

            // Clear the old targets, batches, and effects
            _scrollContainer?.Dispose();
            _spriteBatch?.Dispose();
            _internalEffect?.Dispose();

            // Create a new scrollcontainer and spritebatch
            _scrollContainer = new RenderTarget2D(graphicsDevice, this.Inner.LocalBounds.Width, this.Inner.LocalBounds.Height);
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _internalEffect = new BasicEffect(graphicsDevice)
            {
                TextureEnabled = true,
                View = Matrix.Identity
            };
            this.updateProjectionMatrix();

            base.CleanTexture(graphicsDevice, layerRenderTarget, outputRenderTarget, spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Outer.GlobalBounds.Intersects(this.Stage.clientBounds.GlobalBounds) && (this.Parent == null || this.Outer.GlobalBounds.Intersects(this.Parent.Inner.GlobalBounds)))
            { // Draw the container if it is within screen bounds
                // Set the render targets...
                var rTargets = _graphicsDevice.GetRenderTargets();
                _graphicsDevice.SetRenderTarget(_scrollContainer);
                _graphicsDevice.Clear(Color.Transparent);

                // Draw onto the spritebatch...
                _spriteBatch.Begin(effect: _internalEffect);
                // Ensure that every self contained child element gets drawn too...
                foreach (Element child in this.children)
                    child.Draw(_spriteBatch);
                _spriteBatch.End();

                // Reset the original render targets...
                _graphicsDevice.SetRenderTargets(rTargets);

                // Draw the container...
                if (texture != null) // Draw the texture, if there is one
                    spriteBatch.Draw(texture, this.Outer.GlobalBounds, Color.White);

                spriteBatch.Draw(_scrollContainer, this.Inner.GlobalBounds.Location.ToVector2(), Color.White);
            }
        }

        private void updateProjectionMatrix()
        {
            _internalEffect.Projection = Matrix.CreateTranslation(0.5f, 0.5f, 0) *
                    Matrix.CreateOrthographicOffCenter(
                        this.Inner.GlobalBounds.Left,
                        this.Inner.GlobalBounds.Right,
                        this.Inner.GlobalBounds.Bottom,
                        this.Inner.GlobalBounds.Top,
                        0f,
                        1f);
        }

        public void ScrollBy(Single amount)
        {
            this.Scroll += amount;

            if (this.Scroll < 0)
                this.Scroll = 0;
            else if (this.Scroll > 1)
                this.Scroll = 1;

            this.Items.DirtyPosition = true;
            this.ScrollBar.DirtyPosition = true;

            this.OnScrolled?.Invoke(this, this);
        }
    }
}
