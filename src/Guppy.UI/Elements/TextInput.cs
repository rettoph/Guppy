using Guppy.DependencyInjection;
using Guppy.UI.Enums;
using Guppy.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    public class TextInput : TextElement
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        private GameWindow _window;
        private SpriteBatch _spriteBatch;
        private Texture2D _carret;
        private ActionTimer _carretTimer;
        private Boolean _renderCarret;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Inline = InlineType.None;

            provider.Service(out _graphics);
            provider.Service(out _window);
            provider.Service(out _spriteBatch);

            _carret = _graphics.BuildPixel();
            _carretTimer = new ActionTimer(750);

            this.OnState[ElementState.Focused] += this.HandleFocused;
        }

        protected override void Release()
        {
            base.Release();

            _window.TextInput -= this.HandleTextInput;
            this.OnDraw -= this.DrawCarret;

            _graphics = null;
            _window = null;
            _spriteBatch = null;
        }
        #endregion

        #region Frame Methods
        private void DrawCarret(GameTime gameTime)
        {
            _carretTimer.Update(gameTime, gt => _renderCarret = !_renderCarret);

            if(_renderCarret)
            {
                var textSize = this.Font.MeasureString(this.Value);
                var carretSize = this.Font.MeasureString("|").Y;
                var alignment = this.Alignment.Align(this.Font.MeasureString(this.Value), this.InnerBounds);
                var pos = new Vector2(
                    x: alignment.X + textSize.X, 
                    y: alignment.Y + (textSize.Y / 2) - (carretSize / 2));

                _spriteBatch.Draw(
                    _carret, 
                    new Rectangle(
                        (Int32)pos.X,
                        (Int32)pos.Y, 
                        1, 
                        (Int32)carretSize), 
                    this.Color);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handle the element focused state changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="which"></param>
        /// <param name="value"></param>
        private void HandleFocused(IElement sender, ElementState which, bool value)
        {
            if(value)
            {
                _window.TextInput += this.HandleTextInput;
                this.OnDraw += this.DrawCarret;

                _renderCarret = false;
                _carretTimer.Reset();
            }
            else
            {
                _window.TextInput -= this.HandleTextInput;
                this.OnDraw -= this.DrawCarret;
            }
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            switch(e.Key)
            {
                case Keys.Back:
                    if(this.Value.Length > 0)
                        this.Value = this.Value.Substring(0, this.Value.Length - 1);
                    break;
                default:
                    if(this.Font.Characters.Contains(e.Character))
                        this.Value += e.Character;
                    break;
            }
        }
        #endregion
    }
}
