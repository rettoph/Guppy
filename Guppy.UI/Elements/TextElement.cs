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
    public class TextElement : SimpleElement
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
        private Vector2 _textPosition;
        protected Vector2 textPosition { get { return _textPosition; } }

        public TextElement(Unit x, Unit y, Unit width, Unit height, String text = "", StyleSheet rootStyleSheet = null) : base(x, y, width, height, rootStyleSheet)
        {
            this.Text = text;

            _textPosition = new Vector2(0, 0);
        }

        protected override void generateTexture(ElementState state, ref RenderTarget2D target)
        {
            var font = this.StyleSheet.GetProperty<SpriteFont>(state, StyleProperty.Font);
            var color = this.StyleSheet.GetProperty<Color>(state, StyleProperty.FontColor);
            var alignment = this.StyleSheet.GetProperty<Alignment>(state, StyleProperty.TextAlignment);


            if (font != null)
            {
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

                // Load required assets for text loading
                this.textBounds = font.MeasureString(this.Text);

                if (target.Width - padLeft - padRight > 0 && target.Height - padTop - padBottom > 0)
                { // Only draw the text if the text target is greater than 0
                    var textTarget = new RenderTarget2D(this.graphicsDevice, target.Width - padLeft - padRight, target.Height - padTop - padBottom);

                    // Begin work on text snipper rendering...
                    this.graphicsDevice.SetRenderTarget(textTarget);
                    this.graphicsDevice.Clear(Color.Transparent);

                    /*
                     * Horizontal Alignment Logic
                     */
                    if ((alignment & Alignment.Left) != 0 && this.textBounds.X < textTarget.Width)
                    {
                        _textPosition.X = 0;
                    }
                    else if ((alignment & Alignment.HorizontalCenter) != 0 && this.textBounds.X < textTarget.Width)
                    {
                        _textPosition.X = (Int32)((textTarget.Width - this.textBounds.X) / 2);
                    }
                    else if ((alignment & Alignment.Right) != 0 || this.textBounds.X > textTarget.Width)
                    {
                        _textPosition.X = textTarget.Width - this.textBounds.X;
                    }

                    /*
                     * Vertical Alignment Logic
                     */
                    if ((alignment & Alignment.Top) != 0)
                    {
                        _textPosition.Y = 0;
                    }
                    else if ((alignment & Alignment.VerticalCenter) != 0)
                    {
                        _textPosition.Y = (Int32)((textTarget.Height - font.LineSpacing) / 2);
                    }
                    else if ((alignment & Alignment.Bottom) != 0)
                    {
                        _textPosition.Y = textTarget.Height - font.LineSpacing;
                    }


                    // Draw the text onto the text target...
                    this.internalSpriteBatch.Begin(blendState: BlendState.AlphaBlend);
                    this.internalSpriteBatch.DrawString(font, this.Text, this.textPosition, color);
                    this.internalSpriteBatch.End();

                    // Draw the text target onto the element target...
                    this.graphicsDevice.SetRenderTarget(target);
                    this.graphicsDevice.Clear(Color.Transparent);
                    base.generateTexture(state, ref target);

                    this.internalSpriteBatch.Begin(blendState: BlendState.AlphaBlend);
                    this.internalSpriteBatch.Draw(textTarget, new Vector2(padLeft.Value, padTop.Value), Color.White);
                    this.internalSpriteBatch.End();

                    // Dispose of the text target
                    textTarget?.Dispose();
                }
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
