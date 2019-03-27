using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Special implentation of the Element class
    /// that contains a text value (and will utilize
    /// Text and Font relaated styles within the stylesheet)
    /// </summary>
    public class TextElement : Element
    {
        private String _text;
        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                this.Dirty = true;
            }
        }

        protected Vector2 textBounds { get; private set; }

        public TextElement(Unit x, Unit y, Unit width, Unit height, String text = "", StyleSheet rootStyleSheet = null) : base(x, y, width, height, rootStyleSheet)
        {
            this.Text = text;
        }

        protected override void generateTexture(ElementState state, ref RenderTarget2D target)
        {
            base.generateTexture(state, ref target);

            var font = this.StyleSheet.GetProperty<SpriteFont>(state, StyleProperty.Font);
            var color = this.StyleSheet.GetProperty<Color>(state, StyleProperty.FontColor);
            var alignment = this.StyleSheet.GetProperty<Alignment>(state, StyleProperty.TextAlignment);


            if (font != null && this.Text != String.Empty)
            {
                this.textBounds = font.MeasureString(this.Text);
                Vector2 textPosition = new Vector2(0, 0);

                // Load padding values...
                var padTop = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingTop);
                var padRight = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingRight);
                var padBottom = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingBottom);
                var padLeft = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingLeft);
                // Update padding values...
                padTop.UpdateValue(this.Bounds.Height);
                padRight.UpdateValue(this.Bounds.Width);
                padBottom.UpdateValue(this.Bounds.Height);
                padLeft.UpdateValue(this.Bounds.Width);

                /*
                 * Horizontal Alignment Logic
                 */
                if ((alignment & Alignment.Left) != 0)
                {
                    textPosition.X = padLeft;
                }
                else if ((alignment & Alignment.HorizontalCenter) != 0)
                {
                    textPosition.X = (target.Width - this.textBounds.X) / 2;
                }
                else if ((alignment & Alignment.Right) != 0)
                {
                    textPosition.X = target.Width - this.textBounds.X - padRight;
                }

                /*
                 * Vertical Alignment Logic
                 */
                if ((alignment & Alignment.Top) != 0)
                {
                    textPosition.Y = padTop;
                }
                else if ((alignment & Alignment.VerticalCenter) != 0)
                {
                    textPosition.Y = (target.Height - this.textBounds.Y) / 2;
                }
                else if ((alignment & Alignment.Bottom) != 0)
                {
                    textPosition.Y = target.Height - this.textBounds.Y - padBottom;
                }

                this.internalSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
                this.internalSpriteBatch.DrawString(font, this.Text, textPosition, color);
                this.internalSpriteBatch.End();
            }
        }

        /// <summary>
        /// Generate a list of vertices for the stage debug view
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override VertexPositionColor[] generateDebugVertices(ElementState state)
        {
            var wireframeColor = this.StyleSheet.GetProperty<Color>(state, StyleProperty.DebugWireframeColor);
            var paddingColor = Color.Gray;

            // Load padding values...
            var padTop = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingTop);
            var padRight = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingRight);
            var padBottom = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingBottom);
            var padLeft = this.StyleSheet.GetProperty<Unit>(state, StyleProperty.PaddingLeft);
            // Update padding values...
            padTop.UpdateValue(this.Bounds.Height);
            padRight.UpdateValue(this.Bounds.Width);
            padBottom.UpdateValue(this.Bounds.Height);
            padLeft.UpdateValue(this.Bounds.Width);

            return new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(this.Bounds.Left + padLeft, this.Bounds.Top + padTop, 0), paddingColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right - padRight, this.Bounds.Top + padTop, 0), paddingColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right - padRight, this.Bounds.Top + padTop, 0), paddingColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right - padRight, this.Bounds.Bottom - padBottom, 0), paddingColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right - padRight, this.Bounds.Bottom - padBottom, 0), paddingColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left + padLeft, this.Bounds.Bottom - padBottom, 0), paddingColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left + padLeft, this.Bounds.Bottom - padBottom, 0), paddingColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left + padLeft, this.Bounds.Top + padTop, 0), paddingColor),

                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), wireframeColor),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), wireframeColor)
            };
        }
    }
}
