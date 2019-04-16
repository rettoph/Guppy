using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Utilities.Units.UnitValues;

namespace Guppy.UI.Elements
{
    public class Scrollbar : Element
    {
        protected internal Scrollbar() : base(new UnitValue[] { 1f, -15 }, 0, 15, 1f)
        {
            // Update the blacklist
            this.StateBlacklist = ElementState.Active | ElementState.Hovered | ElementState.Pressed;
        }
    }
}
