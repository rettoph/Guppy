using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units.UnitValues
{
    public class PercentUnitValue : UnitValue
    {
        private Single _amount;

        public PercentUnitValue(Single amount)
        {
            _amount = amount;
        }

        public override int Generate(int parent)
        {
            return (Int32)(_amount * parent);
        }

        protected internal override UnitValue Flip()
        {
            return new PercentUnitValue(-_amount);
        }
    }
}
