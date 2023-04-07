using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class Label : Element
    {
        private IStyle<SpriteFont> _font = null!;
        private IStyle<Color> _color = null!;

        public string Text = string.Empty;

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            base.Initialize(stage, parent);

            _font = this.stage.StyleSheet.Get<SpriteFont>(Property.Font, this);
            _color = this.stage.StyleSheet.Get<Color>(Property.Color, this);
        }

        protected override void DrawContent(GameTime gameTime, Vector2 position)
        {
            base.DrawContent(gameTime, position);

            this.stage.SpriteBatch.DrawString(
                spriteFont: _font.GetValue(this.state), 
                text: this.Text, 
                position: position, 
                color: _color.GetValue(this.state));
        }

        protected override void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            base.CleanContentBounds(constraints, out contentBounds);

            if(string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            if(!_font.TryGetValue(this.state, out var font))
            {
                return;
            }

            Vector2 textSize = font.MeasureString(this.Text);
            contentBounds.Width = textSize.X;
            contentBounds.Height = textSize.Y;
        }
    }
}
