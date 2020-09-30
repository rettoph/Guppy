using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// Represents a dynamic variable unit that
    /// can be transformed into a pixel value
    /// based on a recieved parent pixel size.
    /// </summary>
    public abstract class Unit
    {
        #region Methods
        /// <summary>
        /// Return the pixel specific conversion of ths current unit.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public abstract Int32 ToPixel(Int32 parent);

        /// <summary>
        /// Invert the current unit size
        /// </summary>
        /// <returns></returns>
        public abstract Unit Flip();
        #endregion

        #region Operators
        public static implicit operator Unit(Single amount)
        {
            return new PercentUnit(amount);
        }
        public static implicit operator Unit(Int32 value)
        {
            return new PixelUnit(value);
        }
        public static implicit operator Unit(Func<Int32, Int32> toPixel)
        {
            return new CustomUnit(toPixel);
        }
        public static Unit operator +(Unit a, Unit b)
        {
            return new OperatorUnit(a, b, OperatorUnit.Addition);
        }
        public static Unit operator -(Unit a, Unit b)
        {
            return new OperatorUnit(a, b, OperatorUnit.Subtraction);
        }
        #endregion
    }
}
