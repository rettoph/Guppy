using Guppy.UI.Attributes;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum StyleProperty
    {
        /*
         * Debug Properties
         */
        [StyleProperty(typeof(Color), true)]
        OuterDebugBoundaryColor,
        [StyleProperty(typeof(Color), true)]
        InnerDebugBoudaryColor
    }
}
