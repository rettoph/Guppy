using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public abstract class Unit
    {
        protected Int32 value;

        public abstract void Update(Single bound);

        #region Operators
        public static implicit operator Int32(Unit unit)
        {
            return unit.value;
        }
        public static implicit operator Unit(Int32 value)
        {
            return new PixelUnit(value);
        }
        public static implicit operator Unit(Single value)
        {
            return new PercentUnit(value);
        }
        public static implicit operator Unit(Unit[] values)
        {
            return new NestedUnit(values);
        }
        #endregion
    }
}
