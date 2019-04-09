using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units.UnitValues;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    public class SimpleContainer : Container
    {
        public SimpleContainer(Style style = null) : base(style)
        {
            // By default blacklist all states but normal...
            this.StateBlacklist = ElementState.Active | ElementState.Pressed | ElementState.Hovered;
        }
        public SimpleContainer(UnitValue x, UnitValue y, UnitValue width, UnitValue height, Style style = null) : base(x, y, width, height, style)
        {
            // By default blacklist all states but normal...
            this.StateBlacklist = ElementState.Active | ElementState.Pressed | ElementState.Hovered;
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
