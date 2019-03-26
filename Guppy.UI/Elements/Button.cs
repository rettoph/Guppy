using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class Button : Element
    {
        public Button(Unit x, Unit y, Unit width, Unit height, StyleSheet rootStyleSheet = null) : base(x, y, width, height, rootStyleSheet)
        {
        }
    }
}
