using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// A unit that returns a pixel percentage of
    /// the incoming parent size.
    /// </summary>
    public class PercentUnit : MultiUnit
    {
        #region Private Fields
        private Single _percent;
        #endregion

        #region Constructors
        public PercentUnit(Single percent, params Unit[] padding) : base(padding)
        {
            _percent = percent;
        }
        #endregion

        #region Unit Implementation
        /// <inheritdoc />
        public override Unit Flip()
            => new PercentUnit(-_percent, base.Flip());

        /// <inheritdoc />
        public override int ToPixel(int parent)
            => (Int32)(parent * _percent) + base.ToPixel(parent);

        public override bool Equals(object obj)
        {
            if (obj is PercentUnit p)
                if (p._percent == this._percent)
                    return base.Equals(obj);

            return false;
        }
        #endregion
    }
}
