using Guppy.UI.Entities.UI.Interfaces;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    public class TextButton : StyleElement, ITextElement
    {
        #region Private Fields
        private TextElement _textElement;
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
        #endregion

        #region Events
        public event EventHandler OnClick;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            _textElement = this.add<TextElement>();

            this.EventType = EventTypes.Normal;

            this.OnButtonReleased += this.HandleButtonPressed;
        }

        public override void Dispose()
        {
            base.Dispose();

            _textElement.Dispose();

            this.OnButtonReleased -= this.HandleButtonPressed;
        }
        #endregion

        #region Event Handlers
        private void HandleButtonPressed(object sender, Pointer.Button e)
        {
            if (this.Hovered && (this.stage.released & Pointer.Button.Left) != 0)
                this.OnClick?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
