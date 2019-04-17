using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class TextButton : TextElement
    {
        public TextButton(string text, Unit x, Unit y, Unit width, Unit height, Style style = null) : base(text, x, y, width, height, style)
        {
            this.StateBlacklist = ElementState.Active;
        }
    }
}
