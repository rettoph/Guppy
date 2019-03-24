using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public class PercentUnit : Unit
    {
        private Single _amount;

        public PercentUnit(Single amount)
        {
            _amount = amount;
        }

        public override void UpdateValue(int bound)
        {
            this.Value = (Int32)(_amount * bound);
        }
    }
}
