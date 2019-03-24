using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public class NestedUnit : Unit
    {
        private Unit[] _units;

        public NestedUnit(params Unit[] units)
        {
            _units = units;
        }

        public override void UpdateValue(int bound)
        {
            this.Value = 0;

            foreach(Unit unit in _units)
            {
                unit.UpdateValue(bound);
                this.Value += unit.Value;
            }
        }
    }
}
