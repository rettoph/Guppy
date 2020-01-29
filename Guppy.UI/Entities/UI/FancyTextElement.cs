using Guppy.Extensions.Collection;
using Guppy.UI.Enums;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// An container designed to hold multiple text elements.
    /// 
    /// This will automatically re-adjust their width and x
    /// values to create a seamless looking collection of text.
    /// </summary>
    public class FancyTextElement : Container<TextElement>
    {
        #region Private Fields
        private Alignment _alignment = Alignment.Center;
        /// <summary>
        /// The current alignment offset (used to calculate the internal children offset)
        /// </summary>
        private Point _alignmentOffset;
        #endregion

        #region Public Fields
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

        #region Clean Methods
        protected override void Clean()
        {
            base.Clean();

            Vector2 size = Vector2.Zero;

            this.Children.ForEach(c =>
            {
                var tSize = c.Font.MeasureString(c.Text);

                c.Bounds.X = Unit.Get((Int32)size.X, new CustomUnit(p => _alignmentOffset.X));
                c.Bounds.Y = new CustomUnit(p => _alignmentOffset.Y);
                c.Bounds.Width = (Int32)tSize.X;

                size.X += tSize.X;
                size.Y = Math.Max(size.Y, tSize.Y);
            });

            this.Children.ForEach(c => c.Bounds.Height = (Int32)size.Y);

            _alignmentOffset = this.Align(size, this.Alignment).ToPoint();
        }
        #endregion
    }
}
