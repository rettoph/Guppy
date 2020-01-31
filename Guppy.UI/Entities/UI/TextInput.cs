using Guppy.UI.Enums;
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
    /// Allows users to directly input text via typing.
    /// </summary>
    public class TextInput : ProtectedContainer<TextElement>
    {
        #region Private Fields
        private GameWindow _window;
        private TextElement _text;
        private Double _caretTime;
        private Double _caretInterval = 1;
        #endregion

        #region Public Fields
        public String Value
        {
            get => _text.Text;
            set => _text.Text = value;
        }

        public Alignment Alignment
        {
            get => _text.Alignment;
            set => _text.Alignment = value;
        }

        public SpriteFont Font
        {
            get => _text.Font;
            set => _text.Font = value;
        }

        public Color Color
        {
            get => _text.Color;
            set => _text.Color = value;
        }

        public Unit PadTop { get; set; }

        public Unit PadLeft { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _window = provider.GetRequiredService<GameWindow>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.PadLeft = 7;
            this.PadTop = 0;

            _text = this.add<TextElement>(t =>
            {
                t.Inline = false;
                t.Alignment = Alignment.CenterLeft;
                t.Bounds.Set(
                    x: new CustomUnit(p => this.PadLeft.ToPixel(p)),
                    y: new CustomUnit(p => this.PadTop.ToPixel(p)),
                    width: new CustomUnit(p => p - this.PadLeft.ToPixel(p) - this.PadLeft.ToPixel(p)),
                    height: new CustomUnit(p => p - this.PadTop.ToPixel(p) - this.PadTop.ToPixel(p)));
            });

            this.OnActiveChanged += this.HandleActiveChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            _window.TextInput -= this.HandleTextInput;
            this.OnActiveChanged -= this.HandleActiveChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.Active)
                _caretTime = (_caretTime + gameTime.ElapsedGameTime.TotalSeconds) % _caretInterval;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this.Active && _caretTime < _caretInterval / 2)
            {
                var size = this.Font.MeasureString(this.Value);
                size.Y = Math.Max(Font.LineSpacing, size.Y);
                var position = _text.Align(
                    size,
                    size.X > _text.Bounds.Pixel.Width ? this.Alignment | Alignment.Right : this.Alignment,
                    false);
                this.primitiveBatch.DrawLine(
                    _text.Bounds.Pixel.Location.ToVector2() + position + new Vector2((Int32)size.X + 1, 0),
                    _text.Bounds.Pixel.Location.ToVector2() + position + new Vector2((Int32)size.X + 1, Font.LineSpacing),
                    this.Color);
            }
        }
        #endregion

        #region Event Handlers
        private void HandleActiveChanged(object sender, bool e)
        {
            _caretTime = 0;

            if (e)
                _window.TextInput += this.HandleTextInput;
            else
                _window.TextInput -= this.HandleTextInput;
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            _caretTime = 0;

            switch (e.Key)
            {
                case Keys.Back:
                    if (this.Value.Length > 0)
                        this.Value = this.Value.Substring(0, this.Value.Length - 1);
                    break;
                default:
                    if (this.Font.Characters.Contains(e.Character))
                        this.Value += e.Character;
                    else
                        this.Value += "?";
                    break;
            }
        }
        #endregion
    }
}
