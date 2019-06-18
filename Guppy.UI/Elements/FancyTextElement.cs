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
using Guppy.UI.Utilities;
using Guppy.UI.Entities;

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
        private struct CharColorPosition
        {
            public Char Char;
            public Color Color;
            public Vector2 Position;
        }

        private List<TextColor> _text;

        public FancyTextElement(UnitRectangle outerBounds, Element parent, Stage stage, Style style = null) : base(outerBounds, parent, stage, style)
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
                var position = Vector2.Zero;
                var bounds = Vector2.Zero;
                var charBounds = Vector2.Zero;
                var output = new List<CharColorPosition>();

                foreach (TextColor tc in _text) {
                    foreach (Char tChar in tc.Text.ToCharArray())
                    {
                        // Grab the current char size
                        charBounds = font.MeasureString(tChar.ToString());
                        // If the char overlaps the inner bounds of the element...
                        if(position.X + charBounds.X > this.Inner.LocalBounds.Width)
                        {
                            position.X = 0;
                            position.Y += font.LineSpacing;
                        }

                        // Store the position to the specified char
                        output.Add(new CharColorPosition()
                        {
                            Char = tChar,
                            Color = tc.Color,
                            Position = new Vector2(position.X, position.Y)
                        });

                        // Shift to the next char position
                        position.X += charBounds.X;

                        // If this is the max length position, then update the bounds
                        if (position.X > bounds.X)
                            bounds.X = position.X;
                    }
                }

                bounds.Y = position.Y + font.LineSpacing;
                var tPosition = this.Inner.RelativeBounds.Align(bounds, alignment);


                // Draw the string...
                spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                foreach (CharColorPosition ccp in output)
                    spritebatch.DrawString(font, ccp.Char.ToString(), tPosition + ccp.Position, ccp.Color);
                
                spritebatch.End();

                // Update the element height it inline is set to true
                if(this.Style.Get<Boolean>(GlobalProperty.InlineContent, true))
                    this.Outer.Height.SetValue((Int32)(bounds.Y + (this.Outer.LocalBounds.Height - this.Inner.LocalBounds.Height)));

                // Dispose of resources
                output.Clear();

                // Return the inner element bounds
                return this.Inner.RelativeBounds;
            }

            return Rectangle.Empty;
        }
    }
}
