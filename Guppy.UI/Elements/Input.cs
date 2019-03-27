using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Elements
{
    public class Input : TextElement
    {
        public Boolean Active { get; protected set; }
        public Caret Caret { get; private set; }

        public Input(
            Unit x,
            Unit y,
            Unit width,
            Unit height,
            String text = "",
            StyleSheet rootStyleSheet = null,
            Unit caretWidth = null) : base(x, y, width, height, text, rootStyleSheet)
        {
            this.Active = false;

            this.Caret = new Caret(0, 0, caretWidth ?? 2, 1f);

            this.OnMouseUp += this.HandleMouseUp;
        }

        #region Frame Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.Active)
            {

                if (!this.mouseOver && this.inputManager.Mouse.LeftButton == ButtonState.Pressed)
                { // Mark the current element as inactive
                    this.Active = false;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            this.Caret.Draw(gameTime, spriteBatch);
        }
        #endregion

        #region Method Handlers
        protected override void generateTexture(ElementState state, ref RenderTarget2D target)
        {
            base.generateTexture(state, ref target);

            var caretHeight = (Int32)this.StyleSheet.GetProperty<SpriteFont>(ElementState.Normal, StyleProperty.Font).MeasureString("Lorem Ipsum").Y;
            this.Caret.Height = caretHeight;
        }
        #endregion

        #region Event Handlers
        private void HandleMouseUp(object sender, Element e)
        {
            if(!this.Active)
            {
                this.Active = true;
            }
        }
        #endregion
    }
}
