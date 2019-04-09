using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units.UnitValues
{
    public class PixelUnitValue : UnitValue
    {
        private Int32 _value;

        public PixelUnitValue(Int32 value)
        {
            _value = value;
        }

        public override int Generate(int parent)
        {
            return _value;
        }

        protected internal override UnitValue Flip()
        {
            return new PixelUnitValue(-_value);
        }
    }
}
