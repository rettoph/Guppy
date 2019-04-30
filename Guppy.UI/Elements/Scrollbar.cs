using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Elements
{
    public class Scrollbar : Element
    {
        private Texture2D _texture;
        private ScrollContainer _container;
        private Point _mouseDelta;
        private Point _oldMousePosition;

        public ScrollThumb Thumb { get; private set; }

        public Scrollbar(UnitRectangle outerBounds, ScrollContainer parent, Stage stage) : base(outerBounds, parent, stage)
        {
            _container = parent;
            _oldMousePosition = new Point(0, 0);

            // Update the blacklist
            this.StateBlacklist = ElementState.Active | ElementState.Hovered | ElementState.Pressed;

            this.Thumb = this.createElement<ScrollThumb>(0, 0, 1f, 10);

            this.layers.Add(this.fillColor);

            // Add event handlers
            _container.Items.Outer.Height.OnUpdated += this.HandleHeightUpdated;
            _container.Inner.Height.OnUpdated += this.HandleHeightUpdated;

            this.Thumb.OnStateChanged += this.HandleThumbStateChanged;
        }

        #region Frame Methods
        public override void Update(MouseState mouse)
        {
            base.Update(mouse);

            if(this.Thumb.State == ElementState.Pressed)
            {
                if (_oldMousePosition != Point.Zero)
                {
                    _mouseDelta = mouse.Position - _oldMousePosition;

                    if (_mouseDelta.Y != 0) // Update the scrollbar
                        _container.ScrollBy((Single)_mouseDelta.Y / (this.Inner.Height.Value - this.Thumb.Outer.Height.Value));

                }

                _oldMousePosition = mouse.Position;
            }
        }
        #endregion

        public override void CleanPosition()
        {
            base.CleanPosition();

            this.Thumb.Outer.Y.SetValue((Int32)(_container.Scroll * (this.Inner.Height.Value - this.Thumb.Outer.Height.Value)));
        }

        public override void CleanTexture(GraphicsDevice graphicsDevice, RenderTarget2D layerRenderTarget, RenderTarget2D outputRenderTarget, SpriteBatch spriteBatch)
        {
            if (_texture == null)
                _texture = new Texture2D(graphicsDevice, 1, 1);

            base.CleanTexture(graphicsDevice, layerRenderTarget, outputRenderTarget, spriteBatch);
        }

        private Rectangle fillColor(SpriteBatch spriteBatch)
        {
            _texture.SetData<Color>(new Color[] { _container.Style.Get<Color>(this.State, StateProperty.ScrollBarColor, new Color(240, 240, 240)) });

            spriteBatch.Begin();
            spriteBatch.Draw(_texture, this.Outer.LocalBounds, Color.White);
            spriteBatch.End();

            return this.Outer.LocalBounds;
        }

        #region events
        private void HandleHeightUpdated(object sender, Unit e)
        {
            if (_container.Inner.Height.Value < _container.Items.Outer.Height.Value)
                this.Thumb.Outer.Height.SetValue((Int32)(_container.Outer.Height.Value * ((Single)_container.Inner.Height.Value / (Single)_container.Items.Outer.Height.Value)));
            else
                this.Thumb.Outer.Height.SetValue(0);

            this.Thumb.Outer.Y.SetValue((Int32)(_container.Scroll * (this.Inner.Height.Value - this.Thumb.Outer.Height.Value)));

            this.DirtyPosition = true;
            this.Thumb.DirtyBounds = true;
        }

        private void HandleThumbStateChanged(object sender, Element e)
        {
            if (e.State == ElementState.Pressed)
            {
                _oldMousePosition = Point.Zero;
            }
        }
        #endregion
    }
}
