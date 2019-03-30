using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class ScrollBarHandle : SimpleElement
    {
        private ScrollBar _parent;

        protected internal ScrollBarHandle(ScrollBar parent, StyleSheet rootStyleSheet = null) : base(0, 0, 1f, 1f, rootStyleSheet)
        {
            _parent = parent;
            this.Parent = parent;
        }
    }
}
