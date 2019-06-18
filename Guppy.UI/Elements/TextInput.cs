using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
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
        public static Regex DefaultWhitelist = new Regex(@"[^\t]");

        private Boolean _readingText;
        private Texture2D _caret;
        private Rectangle _caretBounds;
        private Boolean _caretVisible;

        private DateTime _caretToggle;

        public Regex CharWhitelist { get; set; }
        public UInt32 MaxLength { get; set; }

        public event EventHandler<String> OnEnter;

        public TextInput(UnitRectangle outerBounds, Element parent, Stage stage, String text = "", Style style = null) : base(outerBounds, parent, stage, text, style)
        {
            this.CharWhitelist = TextInput.DefaultWhitelist;
            this.MaxLength = UInt32.MaxValue;

            this.OnStateChanged += this.HandleStateChanged;

            _caretBounds = new Rectangle(0, 0, 1, 0);
            _caret = new Texture2D(this.Stage.graphicsDevice, 1, 1);
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

        public override void CleanTexture(GraphicsDevice graphicsDevice, RenderTarget2D layerRenderTarget, RenderTarget2D outputRenderTarget, SpriteBatch spriteBatch)
        {
            base.CleanTexture(graphicsDevice, layerRenderTarget, outputRenderTarget, spriteBatch);

            // Update the text caret
            _caret.SetData<Color>(new Color[] { this.Style.Get<Color>(this.State, StateProperty.TextColor, Color.Black) });
            _caretBounds.X = this.Outer.GlobalBounds.X + (Int32)this.textPosition.X + (Int32)this.textBounds.X;
            _caretBounds.Y = this.Outer.GlobalBounds.Y + (Int32)this.textPosition.Y;
            _caretBounds.Height = (Int32)this.textBounds.Y;
        }

        public override void CleanPosition()
        {
            base.CleanPosition();

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
                this.OnEnter?.Invoke(this, this.Text);
            }
            else if(this.Text.Length < this.MaxLength && e.Character != default(Char) && this.CharWhitelist.IsMatch(e.Character.ToString()))
            {
                this.Text += e.Character;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_readingText)
                this.Stage.TextInput -= this.HandleTextInput;

            _caret?.Dispose();
        }

        public void Select()
        {
            this.setState(ElementState.Active);
        }

        public void Deselect()
        {
            this.setState(ElementState.Normal);
        }
    }
}
