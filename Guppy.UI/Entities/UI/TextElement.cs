using Guppy.Loaders;
using Guppy.UI.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Basic text element designed to contain text and nothing else
    /// No styling, borders, or other customization.
    /// 
    /// Note, the size of this element will dynamically change based
    /// on the size of the internal text value.
    /// </summary>
    public class TextElement : Element
    {
        #region Private Fields
        private Boolean _inline = true;
        private Color _color;
        private SpriteFont _font;
        private String _text;
        private Alignment _alignment;

        private Vector2 _position;

        private RenderTarget2D _renderTarget;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphics;
        #endregion

        #region Public Attributes
        public Boolean Inline {
            get => _inline;
            set {
                _inline = value;
                this.dirty = true;
            }
        }
        public String Text
        {
            get => _text; 
            set
            {
                _text = value;
                this.dirty = true;
            }
        }
        public SpriteFont Font
        {
            get => _font;
            set
            {
                _font = value;
                this.dirty = true;
            }
        }
        public Color Color { get; set; } = Color.White;
        public Alignment Alignment
        {
            get => _alignment; 
            set
            {
                _alignment = value;
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

            // Set defaults
            _text = String.Empty;
            _color = Color.White;
            _alignment = Alignment.TopLeft;
            _font = provider.GetRequiredService<ContentLoader>().TryGet<SpriteFont>("ui:font");
        }
        #endregion

        #region Clean Methods
        protected override void Clean()
        {
            if (this.Inline)
            {
                // Auto update the internal bounds
                var size = this.Font.MeasureString(this.Text);
                this.Bounds.Width = Math.Min((Int32)size.X, this.GetContainerBounds().Width);
                this.Bounds.Height = Math.Min((Int32)size.Y, this.GetContainerBounds().Height);
            }

            base.Clean();

            this.CleanRenderTarget();
        }

        private void CleanRenderTarget()
        {
            // Generate a new render target as needed...
            if (_renderTarget == null || _renderTarget.Bounds.Size != this.Bounds.Pixel.Size)
            {
                _renderTarget?.Dispose();

                if (this.Bounds.Pixel.Width > 0 && this.Bounds.Pixel.Height > 0)
                    _renderTarget = new RenderTarget2D(_graphics, this.Bounds.Pixel.Width, this.Bounds.Pixel.Height);
            }

            if (this.Bounds.Pixel.Width > 0 && this.Bounds.Pixel.Height > 0)
            {
                var targets = _graphics.GetRenderTargets();
                _graphics.SetRenderTarget(_renderTarget);

                _graphics.Clear(Color.Transparent);

                // Draw the text onto the render target...
                _spriteBatch.Begin(blendState: BlendState.Opaque);
                var size = this.Font.MeasureString(this.Text);
                size.Y = Math.Max(Font.LineSpacing, size.Y);
                _spriteBatch.DrawString(
                    this.Font,
                    this.Text,
                    this.Align(
                        size,
                        size.X > this.Bounds.Pixel.Width ? this.Alignment | Alignment.Right : this.Alignment,
                        false),
                    Color.White);
                _spriteBatch.End();

                _graphics.SetRenderTargets(targets);
            }
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw the render target...
            if (_renderTarget != default(RenderTarget2D))
                this.spriteBatch.Draw(_renderTarget, this.Bounds.Pixel.Location.ToVector2(), this.Color);
        }
        #endregion
    }
}
