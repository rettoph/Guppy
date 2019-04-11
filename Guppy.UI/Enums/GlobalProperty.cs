using Guppy.UI.Attributes;
using Guppy.UI.Utilities.Units.UnitValues;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum GlobalProperty
    {
        /*
         * Padding Properties
         */
        [StyleProperty(typeof(UnitValue), true)]
        PaddingTop,
        [StyleProperty(typeof(UnitValue), true)]
        PaddingRight,
        [StyleProperty(typeof(UnitValue), true)]
        PaddingBottom,
        [StyleProperty(typeof(UnitValue), true)]
        PaddingLeft
    }
}
