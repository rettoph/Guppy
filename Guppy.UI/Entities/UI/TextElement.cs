using Guppy.Loaders;
using Guppy.UI.Entities.UI.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Element Specifically designed to render text & nothing else.
    /// No other style options & events are disabled by default
    /// </summary>
    public class TextElement : Element, ITextElement
    {
        #region Private Fields
        private Color _textColor = Color.White;
        private SpriteFont _font;
        private String _text = String.Empty;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphics;
        private RenderTarget2D _renderTarget;
        private RenderTargetBinding[] _initialRenderTargets;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The text alignment.
        /// </summary>
        public BaseElement.Alignment TextAlignment { get; set; } = Alignment.Center;
        /// <summary>
        /// The text value.
        /// </summary>
        public String Text
        {
            get => _text;
            set
            {
                _text = value;
                this.dirty = true;
            }
        }

        /// <summary>
        /// The text font.
        /// </summary>
        public SpriteFont Font
        {
            get => _font;
            set
            {
                _font = value;
                this.dirty = true;
            }
        }

        /// <summary>
        /// The text color.
        /// </summary>
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                this.dirty = true;
            }
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _graphics = provider.GetRequiredService<GraphicsDevice>();
            _spriteBatch = new SpriteBatch(_graphics);

            // Load the default UI font...
            this.Font = provider.GetRequiredService<ContentLoader>().TryGet<SpriteFont>("ui:font");
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.EventType = BaseElement.EventTypes.None;
        }
        #endregion

        #region Clean Methods
        protected override void Clean()
        {
            base.Clean();

            this.CleanRenderTarget();
        }

        private void CleanRenderTarget()
        {
            // Generate a new render target as needed...
            if (_renderTarget == null || _renderTarget.Bounds.Size != this.Bounds.Pixel.Size)
            {
                _renderTarget?.Dispose();

                if (this.Bounds.Pixel.Width <= 0 || this.Bounds.Pixel.Height <= 0)
                    return;

                _renderTarget = new RenderTarget2D(_graphics, this.Bounds.Pixel.Width, this.Bounds.Pixel.Height);
            }

            if (this.Bounds.Pixel.Width > 0 && this.Bounds.Pixel.Height > 0)
            {
                _initialRenderTargets = _graphics.GetRenderTargets();
                _graphics.SetRenderTarget(_renderTarget);

                _graphics.Clear(Color.Transparent);

                // Draw the text onto the render target...
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                var size = this.Font.MeasureString(this.Text);
                size.Y = Math.Max(Font.LineSpacing, size.Y);
                _spriteBatch.DrawString(
                    this.Font,
                    this.Text,
                    this.Align(
                        size,
                        size.X > this.Bounds.Pixel.Width ? this.TextAlignment | Alignment.Right : this.TextAlignment,
                        false),
                    this.TextColor);
                _spriteBatch.End();

                _graphics.SetRenderTargets(_initialRenderTargets);
            }
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw the render target...
            if (this.Bounds.Pixel.Width > 0 && this.Bounds.Pixel.Height > 0)
                this.spriteBatch.Draw(_renderTarget, this.Bounds.Pixel.Location.ToVector2(), Color.White);
        }
        #endregion
    }
}
