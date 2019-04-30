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
    public class Container : Element
    {
        public Container(UnitRectangle outerBounds, Stage stage, Style style = null) : base(outerBounds, stage, style)
        {
            this.StateBlacklist = ElementState.Active | ElementState.Hovered | ElementState.Pressed;
        }

        public virtual void Add(Element child)
        {
            this.add(child);
        }
        public virtual void Remove(Element child)
        {
            this.remove(child);
        }
    }
}
