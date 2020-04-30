using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class TextComponent : Component, ITextElement
    {
        #region Private Fields
        private SpriteFont _font;
        private Color _color;
        private String _text;
        private Alignment _textAlignment;
        #endregion

        #region Public Attributes
        public SpriteFont Font
        {
            get => _font;
            set
            {
                if(value != _font)
                {
                    _font = value;
                    this.OnFontChanged?.Invoke(this, _font);
                }
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                if (value != _color)
                {
                    _color = value;
                    this.OnColorChanged?.Invoke(this, _color);
                }
            }
        }
        public String Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    this.OnTextChanged?.Invoke(this, _text);
                }
            }
        }
        public Alignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                if (value != _textAlignment)
                {
                    _textAlignment = value;
                    this.OnTextAlignmentChanged?.Invoke(this, _textAlignment);
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<SpriteFont> OnFontChanged;
        public event EventHandler<Color> OnColorChanged;
        public event EventHandler<String> OnTextChanged;
        public event EventHandler<Alignment> OnTextAlignmentChanged;
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.SpriteBatch.DrawString(this.Font, this.Text, this.GetTextPosition(), this.Color);
        }
        #endregion
    }
}
