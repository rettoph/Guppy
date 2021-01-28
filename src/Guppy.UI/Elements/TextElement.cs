using Guppy.DependencyInjection;
using Guppy.UI.Enums;
using Guppy.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Guppy.Events.Delegates;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Simple <see cref="Element"/> implication capable
    /// of rendering text.
    /// </summary>
    public class TextElement : Element
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;

        private String _value;
        private SpriteFont _font;
        private InlineType _inlineType;
        #endregion

        #region Public Properties
        /// <summary>
        /// The color with white text should be rendered.
        /// </summary>
        public ElementStateValue<Color> Color;

        /// <summary>
        /// The current text alignment. Stateless.
        /// </summary>
        public Alignment Alignment;

        /// <summary>
        /// The font to render text. Stateless.
        /// </summary>
        public SpriteFont Font
        {
            get => _font;
            set
            {
                if(value != _font)
                {
                    _font = value;
                    this.CleanInlineBounds();
                }
            }
        }

        /// <summary>
        /// The text to be rendered. Stateless.
        /// </summary>
        public String Value
        {
            get => _value ?? "";
            set
            {
                if(value != _value && this.Filter.IsMatch(value))
                {
                    _value = value;
                    this.CleanInlineBounds();
                    this.OnValueChanged?.Invoke(this, _value);
                }
            }
        }

        public Regex Filter { get; set; }

        /// <summary>
        /// When true, the bounds of the internal element
        /// will automatically be adjusted based on the size
        /// of the current <see cref="Value"/>.
        /// </summary>
        public InlineType Inline
        {
            get => _inlineType;
            set {
                _inlineType = value;
                this.CleanInlineBounds();
            }
        }
        #endregion

        #region Events
        public OnEventDelegate<TextElement, String> OnValueChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _graphics);
            provider.Service(out _spriteBatch);

            this.Color = this.BuildStateValue<Color>();
            this.Filter = new Regex("^.{0,500}$");
        }

        protected override void Release()
        {
            base.Release();

            _graphics = null;
            _spriteBatch = null;
            _font = null;
            _value = null;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _graphics.PushScissorRectangle(this.InnerBounds);

            // TODO: Optimize this possibly?
            // I dont think we need to calculate the alignment every frame.
            _spriteBatch.DrawString(
                this.Font,
                this.Value,
                this.Alignment.Align(this.Font.MeasureString(this.Value), this.InnerBounds),
                this.Color);

            _graphics.PopScissorRectangle();
        }
        #endregion

        #region Helper Methods
        private void CleanInlineBounds()
        {
            if((this.Inline & InlineType.Vertical) != 0)
                this.Bounds.Height = new CustomUnit(i => this.Padding.Top.ToPixel(i) + this.Padding.Bottom.ToPixel(i) + (Int32)this.Font.MeasureString(this.Value).Y);
            if ((this.Inline & InlineType.Horizontal) != 0) 
                this.Bounds.Width = new CustomUnit(i => this.Padding.Left.ToPixel(i) + this.Padding.Right.ToPixel(i) + (Int32)this.Font.MeasureString(this.Value).X);
        }

        protected override void Refresh()
        {
            base.Refresh();
        }
        #endregion
    }
}
