using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// A unit that returns a pixel percentage of
    /// the incoming parent size.
    /// </summary>
    public class PercentUnit : Unit
    {
        #region Private Fields
        private Single _percent;
        #endregion

        #region Constructors
        public PercentUnit(Single percent)
        {
            _percent = percent;
        }
        #endregion

        #region Unit Implementation
        /// <inheritdoc />
        public override Unit Flip()
            => new PercentUnit(-_percent);

        /// <inheritdoc />
        public override int ToPixel(int parent)
            => (Int32)(parent * _percent);
        #endregion
    }
}
