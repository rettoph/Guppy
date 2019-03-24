using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public class PixelUnit : Unit
    {
        public PixelUnit(Int32 value)
        {
            this.Value = value;
        }

        public override void UpdateValue(int bound)
        {
            // throw new NotImplementedException();
        }
    }
}
