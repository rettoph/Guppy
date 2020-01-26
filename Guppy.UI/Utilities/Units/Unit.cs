using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// Represents a dynamic variable unit that can
    /// be transformed into a pixel value based on a 
    /// recieved parent size.
    /// </summary>
    public abstract class Unit
    {
        public abstract Int32 ToPixel(Int32 parent);
        public abstract Unit Flip();

        #region Operators
        public static implicit operator Unit(Single amount)
        {
            return new PercentUnit(amount);
        }
        public static implicit operator Unit(Int32 value)
        {
            return new PixelUnit(value);
        }
        public static implicit operator Unit(Unit[] values)
        {
            return new NestedUnit(values);
        }
        public static implicit operator Unit(Func<Int32, Int32> toPixel)
        {
            return new CustomUnit(toPixel);
        }
        public static Unit operator -(Unit uv)
        {
            return uv.Flip();
        }
        #endregion
    }
}
