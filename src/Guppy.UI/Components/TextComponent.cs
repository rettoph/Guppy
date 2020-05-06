using Guppy.DependencyInjection;
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
    public class TextComponent : Component, ITextComponent
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        #endregion

        #region Private Fields
        protected SpriteFont font;
        protected Color color;
        protected String text;
        protected Alignment textAlignment;
        protected Boolean inline;
        #endregion

        #region Public Attributes
        public SpriteFont Font
        {
            get => this.font;
            set
            {
                if(value != this.font)
                {
                    this.font = value;
                    this.OnFontChanged?.Invoke(this, this.font);
                }
            }
        }
        public Color Color
        {
            get => this.color;
            set
            {
                if (value != this.color)
                {
                    this.color = value;
                    this.OnColorChanged?.Invoke(this, this.color);
                }
            }
        }
        public String Text
        {
            get => this.text;
            set
            {
                if (value != this.text)
                {
                    this.text = value;
                    this.OnTextChanged?.Invoke(this, this.text);
                }
            }
        }
        public Alignment TextAlignment
        {
            get => this.textAlignment;
            set
            {
                if (value != this.textAlignment)
                {
                    this.textAlignment = value;
                    this.OnTextAlignmentChanged?.Invoke(this, this.textAlignment);
                }
            }
        }

        public Boolean Inline
        {
            get => this.inline;
            set
            {
                if (value != this.inline)
                {
                    this.inline = value;
                    this.OnInlineChanged?.Invoke(this, this.inline);
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<SpriteFont> OnFontChanged;
        public event EventHandler<Color> OnColorChanged;
        public event EventHandler<String> OnTextChanged;
        public event EventHandler<Alignment> OnTextAlignmentChanged;
        public event EventHandler<Boolean> OnInlineChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);


            provider.Service(out _graphics);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.OnInlineChanged += this.HandleInlineChanged;
            this.TrackInlineEvents(this.Inline);
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnInlineChanged -= this.HandleInlineChanged;
            this.TrackInlineEvents(false);
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var scissor = _graphics.ScissorRectangle;
            _graphics.ScissorRectangle = this.Bounds.Pixel;
            this.spriteBatch.DrawString(this.Font, this.Text, this.GetTextPosition(), this.Color);
            _graphics.ScissorRectangle = scissor;
        }
        #endregion

        #region Helper Methods
        private void TrackInlineEvents(Boolean track)
        {
            if (track)
            {
                this.OnTextChanged += this.HandleTextChanged;
                this.OnFontChanged += this.HandleFontChanged;
            }
            else
            {
                this.OnTextChanged -= this.HandleTextChanged;
                this.OnFontChanged -= this.HandleFontChanged;
            }
        }

        /// <summary>
        /// Automatically resize the element to match its inline size.
        /// </summary>
        private void CleanInlineSize()
        {
            // Auto update the internal bounds
            var size = this.Font.MeasureString(this.Text);
            this.Bounds.Width = Math.Min((Int32)size.X, this.Container.Bounds.Pixel.Width);
            this.Bounds.Height = Math.Min((Int32)size.Y, this.Container.Bounds.Pixel.Height);
        }
        #endregion

        #region Event Handlers
        private void HandleInlineChanged(object sender, bool e)
        {
            this.TrackInlineEvents(e);
        }

        private void HandleFontChanged(object sender, SpriteFont e)
        {
            this.CleanInlineSize();
        }

        private void HandleTextChanged(object sender, string e)
        {
            this.CleanInlineSize();
        }
        #endregion
    }
}
