using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Guppy.UI.Utilities.Units.UnitValues
{
    class NestedUnitValue : UnitValue
    {
        private UnitValue[] _units;

        public NestedUnitValue(params UnitValue[] units)
        {
            _units = units;
        }

        public override Int32 Generate(int parent)
        {
            var val = 0;

            foreach (UnitValue uVal in _units)
                val += uVal.Generate(parent);

            return val;
        }

        protected internal override UnitValue Flip()
        {
            return new NestedUnitValue(_units.Select(uv => uv.Flip()).ToArray());
        }
    }
}
