using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Element designed to contain some level in internal styling capabilities
    /// (Background, Border, Ect.)
    /// </summary>
    public class FancyElement : Element
    {
        #region Private Fields
        private Byte _borderSize;
        private Rectangle[] _borderBounds = new Rectangle[4];
        #endregion

        #region Public Attributes
        public BackgroundStyle BackgroundStyle { get; set; } = BackgroundStyle.Center;
        public Texture2D BackgroundImage { get; set; }
        public Color? BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Byte BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                this.dirty = true;
            }
        }
        #endregion

        #region Clean Methods
        protected override void Clean()
        {
            base.Clean();

            this.CleanBorderBounds();
        }

        private void CleanBorderBounds()
        {
            _borderBounds[0] = new Rectangle(this.Bounds.Pixel.Left, this.Bounds.Pixel.Top, this.Bounds.Pixel.Width, this.BorderSize);
            _borderBounds[1] = new Rectangle(this.Bounds.Pixel.Left, this.Bounds.Pixel.Bottom - this.BorderSize, this.Bounds.Pixel.Width, this.BorderSize);
            _borderBounds[2] = new Rectangle(this.Bounds.Pixel.Left, this.Bounds.Pixel.Top + this.BorderSize, this.BorderSize, this.Bounds.Pixel.Height - this.BorderSize - this.BorderSize);
            _borderBounds[3] = new Rectangle(this.Bounds.Pixel.Right - this.BorderSize, this.Bounds.Pixel.Top + this.BorderSize, this.BorderSize, this.Bounds.Pixel.Height - this.BorderSize - this.BorderSize);
        }
        #endregion

        #region Frame Methods
        protected override void PreDraw(GameTime gameTIme)
        {
            base.PreDraw(gameTIme);

            // Render the background color if any.
            if (this.BackgroundColor.HasValue)
                this.primitiveBatch.FillRectangle(this.Bounds, this.BackgroundColor.Value);

            // Draw the background image, if any
            if(this.BackgroundImage != default(Texture2D))
            {
                switch (this.BackgroundStyle) {
                    case BackgroundStyle.Center:
                        this.spriteBatch.Draw(this.BackgroundImage, this.Align(this.BackgroundImage.Bounds, Alignment.Center, true), Color.White);
                        break;
                    case BackgroundStyle.Fill:
                        this.spriteBatch.Draw(this.BackgroundImage, this.Bounds.Pixel, null, Color.White);
                        break;
                }
            }

            // Render the border, if any is requested
            if (this.BorderSize > 0)
            { // Draw the 4 border bounds...
                this.primitiveBatch.FillRectangle(_borderBounds[0], this.BorderColor);
                this.primitiveBatch.FillRectangle(_borderBounds[1], this.BorderColor);
                this.primitiveBatch.FillRectangle(_borderBounds[2], this.BorderColor);
                this.primitiveBatch.FillRectangle(_borderBounds[3], this.BorderColor);
            }
        }
        #endregion
    }
}
