using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public sealed class PercentUnit : Unit
    {
        private Single _percent;

        public PercentUnit(Single percent)
        {
            _percent = percent;
        }

        public override Int32 ToPixel(Int32 parent)
        {
            return (Int32)(parent * _percent);
        }

        public override Unit Flip()
        {
            return new PercentUnit(-_percent);
        }
    }
}
