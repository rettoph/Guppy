﻿using Guppy.DependencyInjection;
using Guppy.UI.Collections;
using Guppy.UI.Entities;
using Guppy.UI.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class ScrollContainer : ProtectedContainer, IContainer
    {
        #region Private Fields
        private StackContainer _container;
        private Cursor _cursor;
        private Boolean _scrolling;
        private Int32 _oldScroll;
        private Int32 _oldWheel;
        private GraphicsDevice _graphics;
        #endregion

        #region Public Attributes
        public ComponentCollection Children => _container.Children;
        public Component Thumb;
        public Single Scroll { get; private set; }
        public Unit ScrollWheelStep { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _cursor);
            provider.Service(out _graphics);

            _container = this.children.Create<StackContainer>();
            _container.Bounds.Set(
                x: 0,
                y: 0,
                width: 1f,
                height: 1f);

            this.ScrollWheelStep = 15;
            this.Thumb = this.children.Create<Component>();
            this.Thumb.Background = Color.Gray;
            this.Thumb.Bounds.Set(
                x: (Unit)1f,
                y: 0,
                width: 15,
                height: new CustomUnit(h =>
                {
                    if (h == 0)
                        return 0;

                    return (Int32)(this.Bounds.Pixel.Height * ((Single)this.Bounds.Pixel.Height / h));
                }));

            this.Thumb.OnButtonPressed += this.HandleThumbButtonPressed;
            this.Thumb.OnButtonReleased += this.HandleThumbButtonReleased;

            this.OnHoveredChanged += this.HandleHoveredChanged;
        }

        protected override void Dispose()
        {
            base.Dispose();

            _cursor.OnScrolled -= this.HandleCursorScrolled;
            this.Thumb.OnButtonPressed -= this.HandleThumbButtonPressed;
            this.Thumb.OnButtonReleased -= this.HandleThumbButtonReleased;
            this.OnHoveredChanged -= this.HandleHoveredChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(_scrolling)
            {
                this.ScrollBy((_cursor.Position.Y - _oldScroll) / (this.Bounds.Pixel.Height - this.Thumb.Bounds.Pixel.Height));
                _oldScroll = (Int32)_cursor.Position.Y;
            }
        }

        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            _graphics.PushScissorRectangle(this.Bounds.Pixel);
        }

        protected override void PostDraw(GameTime gameTime)
        {
            base.PostDraw(gameTime);

            _graphics.PopScissorRectangle();
        }
        #endregion

        #region Helper Methods
        public void ScrollBy(Single delta)
        {
            this.ScrollTo(this.Scroll + delta);
        }
        public Boolean ScrollTo(Single scroll)
        {
            this.Scroll = MathHelper.Clamp(scroll, 0, 1);

            this.Thumb.Bounds.Y = (Int32)MathHelper.Lerp(0, this.Bounds.Pixel.Height - this.Thumb.Bounds.Pixel.Height, this.Scroll);
            _container.Bounds.Y = (Int32)MathHelper.Lerp(0, -_container.Bounds.Pixel.Height + this.Bounds.Pixel.Height, this.Scroll);

            return true;
        }
        #endregion

        #region IBaseContainer Overrides
        protected override Point GetContainerSize()
         => new Point(this.Bounds.Pixel.Width - this.Thumb.Bounds.Pixel.Width, _container.Bounds.Pixel.Height);
        #endregion

        #region Event Handlers
        private void HandleThumbButtonPressed(object sender, Cursor.Button e)
        {
            if(e == Cursor.Button.Left)
            {
                _scrolling = true;
                _oldScroll = (Int32)_cursor.Position.Y;
            }
        }

        private void HandleThumbButtonReleased(object sender, Cursor.Button e)
        {
            if(e == Cursor.Button.Left)
            {
                _scrolling = false;
            }
        }

        private void HandleCursorScrolled(object sender, float e)
        {
            this.ScrollBy(((Single)this.ScrollWheelStep.ToPixel(this.Bounds.Pixel.Height) / this.Bounds.Pixel.Height) * ((_oldWheel - e) / 120));

            _oldWheel = _cursor.Scroll;
        }

        private void HandleHoveredChanged(object sender, bool e)
        {
            if(e)
            {
                _oldWheel = _cursor.Scroll;
                _cursor.OnScrolled += this.HandleCursorScrolled;
            }
            else
            {
                _cursor.OnScrolled -= this.HandleCursorScrolled;
            }
        }
        #endregion
    }
}
