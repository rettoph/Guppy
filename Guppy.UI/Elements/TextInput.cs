using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Structs;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Elements
{
    public class TextInput : TextElement
    {
        private Boolean _caretVisible;
        private DateTime _lastCaretVisibleChange;

        public Caret Caret { get; private set; }

        public TextInput(
            Unit x,
            Unit y,
            Unit width,
            Unit height,
            String text = "",
            StyleSheet rootStyleSheet = null,
            Unit caretWidth = null) : base(x, y, width, height, text, rootStyleSheet)
        {
            _caretVisible = false;
            _lastCaretVisibleChange = DateTime.Now;

            this.Activatable = true;
            this.Caret = new Caret(this, 0, 0, caretWidth ?? 1, 1f, this.StyleSheet.Root);

            this.OnActivated += this.HandleActivated;
            this.OnDeactivated += this.HandleDeactivated;
        }

        #region Frame Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.State == ElementState.Active)
            {
                if (DateTime.Now.Subtract(_lastCaretVisibleChange).TotalMilliseconds > 750)
                {
                    _lastCaretVisibleChange = DateTime.Now;
                    _caretVisible = !_caretVisible;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if(_caretVisible)
                this.Caret.Draw(gameTime, spriteBatch);
        }
        #endregion

        #region Method Overriders
        protected internal override void UpdateCache()
        {
            base.UpdateCache();

            var padLeft = this.StyleSheet.GetProperty<Unit>(this.State, StyleProperty.PaddingLeft);
            var padTop = this.StyleSheet.GetProperty<Unit>(this.State, StyleProperty.PaddingTop);

            this.Caret.X = (Int32)(padLeft + this.textPosition.X + this.textBounds.X);
            this.Caret.Y = (Int32)(padTop + this.textPosition.Y);
            this.Caret.UpdateBounds(this.Bounds);

            this.Caret.UpdateBounds(this.Bounds);
            this.Caret.UpdateCache();
        }
        protected internal override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            base.AddDebugVertices(ref vertices);

            if(_caretVisible)
                this.Caret.AddDebugVertices(ref vertices);
        }
        #endregion

        #region Method Handlers
        protected override void generateTexture(ElementState state, ref RenderTarget2D target)
        {
            base.generateTexture(state, ref target);

            var caretHeight = (Int32)this.StyleSheet.GetProperty<SpriteFont>(ElementState.Normal, StyleProperty.Font).MeasureString("Lorem Ipsum").Y;
            this.Caret.Height = caretHeight;
        }
        protected internal override void UpdateBounds()
        {
            base.UpdateBounds();

            this.Caret.UpdateBounds();
        }
        #endregion

        #region Event Handlers
        private void HandleActivated(object sender, Element e)
        {
            this.window.TextInput += this.HandleTextInput;
        }
        private void HandleDeactivated(object sender, Element e)
        {
            this.window.TextInput -= this.HandleTextInput;
            _caretVisible = false;
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            if (e.Key == Keys.Back)
            {
                if (this.Text.Length > 0)
                {
                    this.Text = this.Text.Substring(0, (this.Text.Length - 1));
                    this.Dirty = true;
                }
            }
            else if (e.Character != default(Char))
            {
                _lastCaretVisibleChange = DateTime.Now;
                this.Text += e.Character;
                this.Dirty = true;
            }
        }
        #endregion
    }
}
