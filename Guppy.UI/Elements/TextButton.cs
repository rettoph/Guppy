using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class TextButton : TextElement
    {
        private Boolean _mouseDown;
        public event EventHandler<TextButton> OnClicked;

        public TextButton(UnitRectangle outerBounds, Stage stage, string text = "", Style style = null) : base(outerBounds, stage, text, style)
        {
            this.StateBlacklist = ElementState.Active;

            this.OnStateChanged += this.HandleStateChanged;
        }

        private void HandleStateChanged(object sender, Element e)
        {
            if(this.State == ElementState.Pressed)
                _mouseDown = true;
            else if(this.State == ElementState.Hovered && _mouseDown)
            {
                _mouseDown = false;
                this.OnClicked?.Invoke(this, this);
            }
            else
            {
                _mouseDown = false;
            }
        }
    }
}
