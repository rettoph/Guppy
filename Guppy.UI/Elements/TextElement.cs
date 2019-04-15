using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Basic element that allows for custom text
    /// values.
    /// </summary>
    public class TextElement : Element
    {
        private String _text;

        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                this.DirtyBounds = true;
            }
        }

        public TextElement(String text, Unit x, Unit y, Unit width, Unit height, Style style = null) : base(x, y, width, height, style)
        {
            this.Text = text;

            this.layers.Add(this.drawText);
        }

        private Rectangle drawText(SpriteBatch spritebatch)
        {
            var font = this.Style.Get<SpriteFont>(this.State, StateProperty.Font, this.Stage.font);

            if(font != null)
            {
                var alignment = this.Style.Get<Alignment>(this.State, StateProperty.TextAlignment, Alignment.TopLeft);
                var color = this.Style.Get<Color>(this.State, StateProperty.TextColor, Color.Black);
                var tBounds = font.MeasureString(this.Text);
                tBounds.Y = font.LineSpacing;
                var tPosition = this.Inner.RelativeBounds.Align(tBounds, alignment);

                // Draw the string...
                spritebatch.Begin();
                spritebatch.DrawString(font, this.Text, tPosition, color);
                spritebatch.End();

                // Return the inner element bounds
                return this.Inner.RelativeBounds;
            }

            return Rectangle.Empty;
        }
    }
}
