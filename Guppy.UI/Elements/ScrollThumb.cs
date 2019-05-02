using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    public class ScrollThumb : Element
    {
        private Texture2D _thumbTexture;

        private Scrollbar _container;

        public ScrollThumb(UnitRectangle outerBounds, Scrollbar parent, Stage stage) : base(outerBounds, parent, stage)
        {
            _container = parent;

            this.StateBlacklist = ElementState.Active;

            this.layers.Add(this.fillColor);
        }

        public override void CleanTexture(GraphicsDevice graphicsDevice, RenderTarget2D layerRenderTarget, RenderTarget2D outputRenderTarget, SpriteBatch spriteBatch)
        {
            if (_thumbTexture == null)
                _thumbTexture = new Texture2D(graphicsDevice, 1, 1);

            base.CleanTexture(graphicsDevice, layerRenderTarget, outputRenderTarget, spriteBatch);
        }

        private Rectangle fillColor(SpriteBatch spriteBatch)
        {
            _thumbTexture.SetData<Color>(new Color[] { _container.Style.Get<Color>(this.State, StateProperty.ScrollBarThumbColor, new Color(205, 205, 205)) });

            spriteBatch.Begin();
            spriteBatch.Draw(_thumbTexture, this.Outer.LocalBounds, Color.White);
            spriteBatch.End();

            return this.Outer.LocalBounds;
        }

        public override void Dispose()
        {
            base.Dispose();

            _container = null;

            _thumbTexture.Dispose();
        }
    }
}
