using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework.Input;
using Guppy.UI.Entities;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units.UnitValues;

namespace Guppy.UI.Elements
{
    public class ScrollContainer : Element
    {
        #region Private Fields
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _scrollContainer;
        private BasicEffect _internalEffect;
        private GraphicsDevice _graphicsDevice;
        private Int32 _oldScrollValue;
        private Int32 _scrollDelta;
        #endregion

        #region Public Fields
        public Single Scroll { get; private set; }
        public ScrollItems Items { get; private set; }
        public Scrollbar ScrollBar { get; private set; }
        #endregion

        #region Events
        public event EventHandler<ScrollContainer> OnScrolled;
        #endregion

        public ScrollContainer(
            UnitRectangle outerBounds,
            Element parent,
            Stage stage,
            Style style = null) : base(outerBounds, parent, stage, style)
        {
            this.Items = this.createElement<ScrollItems>(0, 0, new UnitValue[] { 1f, -15 }, 1f);
            this.ScrollBar = this.createElement<Scrollbar>(new UnitValue[] { 1f, -15 }, 0, 15, 1f);

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
                this.Items.Draw(_spriteBatch);
                _spriteBatch.End();

                // Reset the original render targets...
                _graphicsDevice.SetRenderTargets(rTargets);

                // Draw the container...
                if (texture != null) // Draw the texture, if there is one
                    spriteBatch.Draw(texture, this.Outer.GlobalBounds, Color.White);

                spriteBatch.Draw(_scrollContainer, this.Inner.GlobalBounds.Location.ToVector2(), Color.White);
                this.ScrollBar.Draw(spriteBatch);
            }
        }

        private void updateProjectionMatrix()
        {
            _internalEffect.Projection = Matrix.CreateTranslation(0.5f, -0.5f, 0) *
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

        public void ScrollTo(Single value)
        {
            this.Scroll = value;

            if (this.Scroll < 0)
                this.Scroll = 0;
            else if (this.Scroll > 1)
                this.Scroll = 1;

            this.Items.DirtyPosition = true;
            this.ScrollBar.DirtyPosition = true;

            this.OnScrolled?.Invoke(this, this);
        }

        public override void Update(MouseState mouse)
        {
            base.Update(mouse);

            if(this.MouseOver && this.Items.Outer.LocalBounds.Height > this.Outer.LocalBounds.Height)
            {
                _scrollDelta = mouse.ScrollWheelValue - _oldScrollValue;

                if (_scrollDelta > 120)
                    _scrollDelta = 120;
                else if (_scrollDelta < -120)
                    _scrollDelta = -120;

                _oldScrollValue = mouse.ScrollWheelValue;

                if(_scrollDelta != 0)
                    this.ScrollBy((Single)_scrollDelta / (this.Inner.Height.Value - this.Items.Outer.Height.Value));
            }
        }
    }
}
