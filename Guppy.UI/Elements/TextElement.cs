using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
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
                this.Dirty = true;
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
                var tPosition = Vector2.Zero;

                // Horizontal alignment of text here...
                if((alignment & Alignment.Left) != 0)
                { // Left alignment...
                    tPosition.X = this.Inner.RelativeBounds.Left;
                }
                else if ((alignment & Alignment.HorizontalCenter) != 0)
                { // Center alignment...
                    tPosition.X = this.Inner.RelativeBounds.Center.ToVector2().X - (tBounds.X / 2);
                }
                else if ((alignment & Alignment.Right) != 0)
                { // Right alignment...
                    tPosition.X = this.Inner.RelativeBounds.Right - tBounds.X;
                }

                // Horizontal Vertical of text here...
                if ((alignment & Alignment.Top) != 0)
                { // Top alignment...
                    tPosition.Y = this.Inner.RelativeBounds.Top;
                }
                else if ((alignment & Alignment.VerticalCenter) != 0)
                { // Center alignment...
                    tPosition.Y = this.Inner.RelativeBounds.Center.ToVector2().Y - (font.LineSpacing / 2);
                }
                else if ((alignment & Alignment.Bottom) != 0)
                { // Bottom alignment...
                    tPosition.Y = this.Inner.RelativeBounds.Bottom - font.LineSpacing;
                }

                // Draw the string...
                spritebatch.Begin(blendState: BlendState.Additive);
                spritebatch.DrawString(font, this.Text, tPosition, color);
                spritebatch.End();

                // Return the inner element bounds
                return this.Inner.RelativeBounds;
            }

            return Rectangle.Empty;
        }
    }
}
