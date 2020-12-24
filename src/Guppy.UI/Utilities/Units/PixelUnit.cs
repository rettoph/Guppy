using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// A simple static pixel value that
    /// never changes.
    /// </summary>
    public class PixelUnit : Unit
    {
        #region Private Fields
        private Int32 _value;
        #endregion

        #region Constructors
        public PixelUnit(Int32 value)
        {
            _value = value;
        }
        #endregion

        #region Unit Implementation
        /// <inheritdoc />
        public override Unit Flip()
            => new PixelUnit(_value);

        /// <inheritdoc />
        public override int ToPixel(int parent)
            => _value;

        public override bool Equals(object obj)
        {
            if (obj is PixelUnit p)
                return p._value == this._value;

            return false;
        }
        #endregion
    }
}
