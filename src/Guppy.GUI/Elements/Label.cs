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
        private string _textValue;
        private Color? _colorValue;
        private IStyle<SpriteFont> _font = null!;
        private IStyle<Color> _color = null!;

        public string Text
        {
            get => _textValue;
            set
            {
                _textValue = value;

                if(this.Initialized)
                {
                    this.Clean();
                }
            }
        }

        public SpriteFont? Font => _font.GetValue(this.State);
        public Color Color
        {
            get => _colorValue ?? _color.GetValue(this.State);
            set => _colorValue = value;
        }

        public Label(params string[] names) : this((IEnumerable<string>)names)
        {
        }
        public Label(IEnumerable<string> names) : base(names)
        {
            _textValue = string.Empty;
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
                color: this.Color);
        }

        protected override void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            base.CleanContentBounds(constraints, out contentBounds);

            if (!_font.TryGetValue(this.State, out var font))
            {
                return;
            }

            Vector2 textSize = font.MeasureString(this.Text);
            contentBounds.Width = textSize.X;
            contentBounds.Height = MathF.Max(font.LineSpacing, textSize.Y);
        }
    }
}
