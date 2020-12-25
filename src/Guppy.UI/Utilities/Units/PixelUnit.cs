using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// A simple static pixel value that
    /// never changes.
    /// </summary>
    public class PixelUnit : MultiUnit
    {
        #region Private Fields
        private Int32 _value;
        #endregion

        #region Constructors
        public PixelUnit(Int32 value, params Unit[] padding) : base(padding)
        {
            _value = value;
        }
        #endregion

        #region Unit Implementation
        /// <inheritdoc />
        public override Unit Flip()
            => new PixelUnit(-_value, base.Flip());

        /// <inheritdoc />
        public override int ToPixel(int parent)
            => _value + base.ToPixel(parent);

        public override bool Equals(object obj)
        {
            if (obj is PixelUnit p)
                if (p._value == this._value)
                    return base.Equals(obj);

            return false;
        }
        #endregion
    }
}
