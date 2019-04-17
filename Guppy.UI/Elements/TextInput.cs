using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Simple text input element.
    /// </summary>
    public class TextInput : TextElement
    {
        private Boolean _readingText;
        private Texture2D _caret;
        private Rectangle _caretBounds;
        private Boolean _caretVisible;

        private DateTime _caretToggle;

        public TextInput(string text, Unit x, Unit y, Unit width, Unit height, Style style = null) : base(text, x, y, width, height, style)
        {
            this.OnStateChanged += this.HandleStateChanged;

            _caretBounds = new Rectangle(0, 0, 1, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(_caretVisible)
                spriteBatch.Draw(_caret, _caretBounds, Color.White);
        }

        public override void Update(MouseState mouse)
        {
            base.Update(mouse);

            if(this.State == ElementState.Active && DateTime.Now.Subtract(_caretToggle).TotalMilliseconds > 750)
            {
                _caretToggle = DateTime.Now;
                _caretVisible = !_caretVisible;
            }
        }

        protected override void setParent(Element parent)
        {
            base.setParent(parent);

            if (this.Parent != null)
            {
                _caret?.Dispose();
                _caret = new Texture2D(this.Stage.graphicsDevice, 1, 1);
            }
        }

        public override void CleanTexture(GraphicsDevice graphicsDevice, RenderTarget2D layerRenderTarget, RenderTarget2D outputRenderTarget, SpriteBatch spriteBatch)
        {
            base.CleanTexture(graphicsDevice, layerRenderTarget, outputRenderTarget, spriteBatch);

            // Update the text caret
            _caret.SetData<Color>(new Color[] { this.Style.Get<Color>(this.State, StateProperty.TextColor, Color.Black) });
            _caretBounds.X = this.Outer.GlobalBounds.X + (Int32)this.textPosition.X + (Int32)this.textBounds.X;
            _caretBounds.Y = this.Outer.GlobalBounds.Y + (Int32)this.textPosition.Y;
            _caretBounds.Height = (Int32)this.textBounds.Y;
        }

        private void HandleStateChanged(object sender, Element e)
        {
            if (this.State == ElementState.Active && !_readingText) {
                this.Stage.TextInput += this.HandleTextInput;
                _readingText = true;
                _caretVisible = true;
                _caretToggle = DateTime.Now;
            }
            else if(_readingText)
            {
                this.Stage.TextInput -= this.HandleTextInput;
                _readingText = false;
                _caretVisible = false;
            }
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            if(e.Key == Keys.Back)
            {
                if(this.Text.Length > 0)
                    this.Text = this.Text.Remove(this.Text.Length - 1);
            }
            else if(e.Key == Keys.Enter)
            {

            }
            else
            {
                this.Text += e.Character;
            }
        }
    }
}
