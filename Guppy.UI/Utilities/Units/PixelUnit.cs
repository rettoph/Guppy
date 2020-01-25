using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public sealed class PixelUnit : Unit
    {
        private Int32 _value;

        public PixelUnit(Int32 value)
        {
            _value = value;
        }

        public override Int32 ToPixel(Int32 parent)
        {
            return _value;
        }

        public override Unit Flip()
        {
            return new PixelUnit(-_value);
        }
    }
}
