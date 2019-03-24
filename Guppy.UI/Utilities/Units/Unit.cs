using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public abstract class Unit
    {
        public Int32 Value { get; protected set; }

        public abstract void UpdateValue(Int32 bound);

        #region Operators
        public static implicit operator Unit(Int32 value)
        {
            return new PixelUnit(value);
        }
        
        public static implicit operator Unit(Single amount)
        {
            return new PercentUnit(amount);
        }

        public static implicit operator Unit(Unit[] units)
        {
            return new NestedUnit(units);
        }

        public static implicit operator Int32(Unit unit)
        {
            return unit.Value;
        }
        #endregion
    }
}
