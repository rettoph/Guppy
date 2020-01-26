using Guppy.UI.Entities.UI.Interfaces;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Text element designed to accept user 
    /// input.
    /// </summary>
    public class TextInput : Input, ITextElement
    {
        #region Private Fields
        private GameWindow _window;
        private TextElement _textElement;
        private Double _blinkerInterval;
        #endregion

        #region Public Attributes
        public Element.Alignment TextAlignment
        {
            get => _textElement.TextAlignment;
            set => _textElement.TextAlignment = value;
        }

        public String Text
        {
            get => _textElement.Text;
            set => _textElement.Text = value;
        }

        public SpriteFont Font
        {
            get => _textElement.Font;
            set => _textElement.Font = value;
        }

        public Color TextColor
        {
            get => _textElement.TextColor;
            set => _textElement.TextColor = value;
        }

        public Byte PaddingTop { get; set; } = 0;
        public Byte PaddingLeft { get; set; } = 7;
        #endregion

        #region Life Cycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _window = provider.GetRequiredService<GameWindow>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            _textElement = this.add<TextElement>(te =>
            {
                te.TextAlignment = Alignment.CenterLeft;

                te.Bounds.Left = new CustomUnit((p) => this.BorderSize + this.PaddingLeft);
                te.Bounds.Top = new CustomUnit((p) => this.BorderSize + this.PaddingTop);
                te.Bounds.Width = new Unit[] { 1f, new CustomUnit((p) => -2 * (this.BorderSize + this.PaddingLeft)) };
                te.Bounds.Height = new Unit[] { 1f, new CustomUnit((p) => -2 * (this.BorderSize + this.PaddingTop)) };
            });


            this.OnActiveChanged += this.HandleActiveChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            _textElement.Dispose();

            _window.TextInput -= this.HandleTextInput;
            this.OnActiveChanged -= this.HandleActiveChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(this.Active)
                _blinkerInterval = (_blinkerInterval + gameTime.ElapsedGameTime.TotalSeconds) % 1;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this.Active && _blinkerInterval < 0.5f)
            {
                var size = this.Font.MeasureString(this.Text);
                size.Y = Math.Max(Font.LineSpacing, size.Y);
                var position = _textElement.Align(
                    size,
                    size.X > _textElement.Bounds.Pixel.Width ? this.TextAlignment | Alignment.Right : this.TextAlignment,
                    false);
                this.primitiveBatch.DrawLine(
                    _textElement.Bounds.Pixel.Location.ToVector2() + position + new Vector2(size.X + 1, 0),
                    _textElement.Bounds.Pixel.Location.ToVector2() + position + new Vector2(size.X + 1, Font.LineSpacing),
                    this.TextColor);
            }
        }
        #endregion

        #region Event Handlers
        private void HandleActiveChanged(object sender, bool e)
        {
            _blinkerInterval = 0;

            if (e)
                _window.TextInput += this.HandleTextInput;
            else
                _window.TextInput -= this.HandleTextInput;
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            _blinkerInterval = 0;

            switch(e.Key)
            {
                case Keys.Back:
                    if(this.Text.Length > 0)
                        this.Text = this.Text.Substring(0, this.Text.Length - 1);
                    break;
                default:
                    if(this.Font.Characters.Contains(e.Character))
                        this.Text += e.Character;
                    break;
            }
            
        }
        #endregion
    }
}
