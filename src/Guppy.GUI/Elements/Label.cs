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
        private string _text;
        private IStyle<SpriteFont> _font = null!;
        private IStyle<Color> _color = null!;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                this.Clean();
            }
        }

        public SpriteFont? Font => _font.GetValue(this.State);
        public Color? Color => _color.GetValue(this.State);

        public Label(params string[] names) : base(names)
        {
            _text = string.Empty;
        }

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            base.Initialize(stage, parent);

            _font = this.stage.StyleSheet.Get<SpriteFont>(Property.Font, this);
            _color = this.stage.StyleSheet.Get<Color>(Property.Color, this);
        }

        protected override void DrawContent(GameTime gameTime, Vector2 position)
        {
            base.DrawContent(gameTime, position);

            position.Round();

           this.stage.SpriteBatch.DrawString(
                spriteFont: _font.GetValue(this.State), 
                text: this.Text, 
                position: position, 
                color: _color.GetValue(this.State));
        }

        protected override void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            base.CleanContentBounds(constraints, out contentBounds);

            if(string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            if(!_font.TryGetValue(this.State, out var font))
            {
                return;
            }

            Vector2 textSize = font.MeasureString(this.Text);
            contentBounds.Width = textSize.X;
            contentBounds.Height = textSize.Y;
        }
    }
}
