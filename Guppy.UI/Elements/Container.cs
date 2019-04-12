using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class Container : Element
    {
        public Container(Unit x, Unit y, Unit width, Unit height, Style style = null) : base(x, y, width, height, style)
        {
            this.StateBlacklist = ElementState.Active | ElementState.Hovered | ElementState.Pressed;
        }

        public void Add(Element child)
        {
            this.add(child);
        }
        public void Remove(Element child)
        {
            this.remove(child);
        }
    }
}
