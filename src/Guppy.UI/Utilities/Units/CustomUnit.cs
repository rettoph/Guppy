using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public class CustomUnit : Unit
    {
        #region Private Fields
        private Func<Int32, Int32> _calculator;
        private Boolean _flipped;
        #endregion

        #region Constructors
        public CustomUnit(Func<Int32, Int32> calculator)
        {
            _flipped = false;
            _calculator = calculator;
        }
        #endregion

        #region Unit Implementation
        /// <inheritdoc />
        public override Unit Flip()
            => new CustomUnit(_calculator)
            {
                _flipped = !_flipped
            };

        /// <inheritdoc />
        public override int ToPixel(int parent)
            => _calculator(parent) * (_flipped ? -1 : 1);
        #endregion
    }
}
