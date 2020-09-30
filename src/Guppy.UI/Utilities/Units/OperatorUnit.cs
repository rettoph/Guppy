using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// A unit desiged to preform simple arithmetic to
    /// 2 IUnit instances.
    /// </summary>
    public class OperatorUnit : Unit
    {
        #region Private Fields
        private Unit _a;
        private Unit _b;
        private Func<Int32, Int32, Int32> _operation;
        private Boolean _flipped;
        #endregion

        #region Constructors
        public OperatorUnit(Unit a, Unit b, Func<Int32, Int32, Int32> operation)
        {
            _a = a;
            _b = b;
            _operation = operation;
            _flipped = false;
        }
        #endregion

        #region Unit Implementation
        /// <inheritdoc />
        public override Unit Flip()
            => new OperatorUnit(_a, _b, _operation)
            {
                _flipped = !_flipped
            };

        /// <inheritdoc />
        public override int ToPixel(int parent)
            => _operation(_a.ToPixel(parent), _b.ToPixel(parent)) * (_flipped ? -1 : 1);
        #endregion

        #region Static Methods
        public static Int32 Addition(Int32 a, Int32 b)
            => a + b;

        public static Int32 Subtraction(Int32 a, Int32 b)
            => a - b;
        #endregion
    }
}
