using Guppy.DependencyInjection;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class TextInput : ProtectedContainer
    {
        #region Private Fields
        private GameWindow _window;
        private Double _caretTime;
        private Double _caretInterval = 1;
        private PrimitiveBatch _primitiveBatch;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The internal text component that will actually render the input value.
        /// </summary>
        public TextComponent Text { get; private set; }
        public String Value
        {
            get => this.Text.Text;
            set => this.Text.Text = value;
        }
        public UnitRectangle Padding { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _window);
            provider.Service(out _primitiveBatch);

            this.Text = this.children.Create<TextComponent>();
            this.Padding = this.Text.Bounds;
            this.Text.Inline = false;
            this.Text.TextAlignment = Alignment.CenterLeft;

            this.Padding.Top = 3;
            this.Padding.Bottom = 3;
            this.Padding.Left = 7;
            this.Padding.Right = 7;

            this.OnActiveChanged += this.HandleActiveChanged;
        }

        protected override void Dispose()
        {
            base.Dispose();

            _window.TextInput -= this.HandleTextInput;
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
                var size = this.Text.Font.MeasureString(this.Value);
                size.Y = Math.Max(this.Text.Font.LineSpacing, size.Y);
                var position = this.Text.GetTextPosition();

                // TODO: Dont use primitve batch here
                _primitiveBatch.DrawLine(
                    position + new Vector2((Int32)size.X + 1, 0),
                    position + new Vector2((Int32)size.X + 1, this.Text.Font.LineSpacing + 1),
                    this.Text.Color);
            }
        }
        #endregion

        #region Event Handlers
        private void HandleActiveChanged(object sender, bool e)
        {
            if(e)
            {
                _window.TextInput += this.HandleTextInput;
            }
            else
            {
                _window.TextInput -= this.HandleTextInput;
            }
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            switch(e.Key)
            {
                case Keys.Back:
                    if(this.Value.Length > 0)
                        this.Value = this.Value.Remove(this.Value.Length - 1);
                    break;
                default:
                    if(this.Text.Font.Characters.Contains(e.Character))
                        this.Value += e.Character;
                    break;
            }

            // Reset caret blink timer.
            _caretTime = 0;
        }
        #endregion
    }
}
