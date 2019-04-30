using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Utilities.Units.UnitValues;

namespace Guppy.UI.Elements
{
    public class Container : Element
    {
        public Container(UnitRectangle outerBounds, Element parent, Stage stage, Style style = null) : base(outerBounds, parent, stage, style)
        {
            this.StateBlacklist = ElementState.Active | ElementState.Hovered | ElementState.Pressed;
        }

        /// <summary>
        /// Create a new element and automatically add it as a child
        /// of the current element.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual TElement CreateElement<TElement>(UnitValue x, UnitValue y, UnitValue width, UnitValue height, params Object[] args)
            where TElement : Element
        {
            return this.createElement<TElement>(x, y, width, height, args);
        }
        public virtual void Remove(Element child)
        {
            this.remove(child);
        }
    }
}
