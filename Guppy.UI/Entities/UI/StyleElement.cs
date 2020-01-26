using Guppy.UI.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Element that will implement the base attributes & functionality for simple
    /// styling. Background, border, ect...
    /// </summary>
    public abstract class StyleElement : Element
    {
        #region Private Fields
        private Byte _borderSize = 1;
        private Rectangle[] _borderBounds = new Rectangle[4];
        #endregion

        #region Public Attributes
        public Color? BackgroundColor { get; set; }
        public Byte BorderSize
        {
            get => _borderSize;
            set
            {
                if (_borderSize != value)
                {
                    _borderSize = value;
                    this.CleanBorderBounds();
                }
            }
        }
        public Color BorderColor { get; set; } = Color.White;
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            // Render the background color if any.
            if (this.BackgroundColor.HasValue)
                this.primitiveBatch.FillRectangle(this.Bounds, this.BackgroundColor.Value);

            // Render the background, if any is requested
            if (this.BorderSize > 0)
            { // Draw the 4 border bounds...
                this.primitiveBatch.FillRectangle(_borderBounds[0], this.BorderColor);
                this.primitiveBatch.FillRectangle(_borderBounds[1], this.BorderColor);
                this.primitiveBatch.FillRectangle(_borderBounds[2], this.BorderColor);
                this.primitiveBatch.FillRectangle(_borderBounds[3], this.BorderColor);
            }

            // Draw all children..
            base.Draw(gameTime);
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
    }
}
