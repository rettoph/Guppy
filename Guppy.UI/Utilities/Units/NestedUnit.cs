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

        public override void Update(float bound)
        {
            this.value = 0;

            foreach (Unit unit in _units)
            {
                unit.Update(bound);
                this.value += unit;
            }
        }
    }
}
