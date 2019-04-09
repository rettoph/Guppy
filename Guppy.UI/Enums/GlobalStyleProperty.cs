using Guppy.UI.Attributes;
using Guppy.UI.Utilities.Units.UnitValues;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum GlobalStyleProperty
    {
        /*
         * Display Properties
         */

        /// <summary>
        /// Init only
        /// </summary>
        [StyleProperty(typeof(UnitValue), true)]
        X,
        /// <summary>
        /// Init only
        /// </summary>
        [StyleProperty(typeof(UnitValue), true)]
        Y,
        /// <summary>
        /// Init only
        /// </summary>
        [StyleProperty(typeof(UnitValue), true)]
        Width,
        /// <summary>
        /// Init only
        /// </summary>
        [StyleProperty(typeof(UnitValue), true)]
        Height,
        [StyleProperty(typeof(Boolean), false)]
        Inline,

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
        PaddingLeft,
    }
}
