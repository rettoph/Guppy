using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    class NestedUnit : Unit
    {
        private Unit[] _units;

        public NestedUnit(params Unit[] units)
        {
            _units = units;
        }

        public override Int32 ToPixel(Int32 parent)
        {
            var val = 0;

            foreach (Unit uVal in _units)
                val += uVal.ToPixel(parent);

            return val;
        }

        public override Unit Flip()
        {
            return new NestedUnit(_units.Select(uv => uv.Flip()).ToArray());
        }
    }
}
