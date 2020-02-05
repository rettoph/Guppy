using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    public class Button : ProtectedContainer<TextElement>
    {
        #region Private Fields
        private TextElement _text;
        #endregion

        #region Public Attributes
        public String Text
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
        #endregion

        #region Events
        public event EventHandler OnClicked;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            _text = this.add<TextElement>(t =>
            {
                t.Alignment = Alignment.Center;
                t.Inline = false;
            });

            this.OnButtonReleased += this.HandleButtonReleased;
        }
        #endregion

        #region Events
        private void HandleButtonReleased(object sender, Pointer.Button e)
        {
            if (e.HasFlag(Pointer.Button.Left) && this.Hovered)
                this.OnClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
