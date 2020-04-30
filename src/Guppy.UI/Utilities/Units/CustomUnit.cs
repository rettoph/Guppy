using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public class CustomUnit : Unit
    {
        private Func<Int32, Int32> _toPixel;

        protected Boolean flipped;


        public CustomUnit(Func<Int32, Int32> toPixel, Boolean flipped = false)
        {
            _toPixel = toPixel;
        }

        public override Int32 ToPixel(Int32 parent)
        {
            if (flipped)
                return -_toPixel(parent);
            else
                return _toPixel(parent);
        }

        public override Unit Flip()
        {
            return new CustomUnit(_toPixel, !this.flipped);
        }
    }
}
