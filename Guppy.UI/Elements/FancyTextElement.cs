using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Special element that can contain multiple text elements within.
    /// Each text element will automatically be adjusted to remain inline
    /// based on the InlineTextElement's width
    /// </summary>
    public class FancyTextElement : Element
    {
        private struct TextColor
        {
            public String Text;
            public Color Color;
        }

        private List<TextColor> _text;

        public FancyTextElement(Unit x, Unit y, Unit width, Unit height, Style style = null) : base(x, y, width, height, style)
        {
            _text = new List<TextColor>();

            this.layers.Add(this.drawText);
        }

        public void Add(String text, Color color)
        {
            _text.Add(new TextColor()
            {
                Text = text,
                Color = color
            });
        }

        private Rectangle drawText(SpriteBatch spritebatch)
        {
            var font = this.Style.Get<SpriteFont>(this.State, StateProperty.Font, this.Stage.font);

            if (font != null)
            {
                var alignment = this.Style.Get<Alignment>(this.State, StateProperty.TextAlignment, Alignment.TopLeft);
                var color = this.Style.Get<Color>(this.State, StateProperty.TextColor, Color.Black);
                var tBounds = font.MeasureString(String.Join("", _text.Select(tc => tc.Text)));
                tBounds.Y = font.LineSpacing;
                var tPosition = this.Inner.RelativeBounds.Align(tBounds, alignment);


                // Draw the string...
                spritebatch.Begin();

                foreach (TextColor tc in _text)
                {
                    spritebatch.DrawString(font, tc.Text, tPosition, tc.Color);
                    tPosition.X += font.MeasureString(tc.Text).X;
                }
                
                spritebatch.End();

                // Return the inner element bounds
                return this.Inner.RelativeBounds;
            }

            return Rectangle.Empty;
        }
    }
}
