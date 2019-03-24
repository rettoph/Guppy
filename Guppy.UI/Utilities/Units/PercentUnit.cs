using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Guppy.UI.Utilities.Units
{
    public class PercentUnit : Unit
    {
        private Single _value;

        public PercentUnit(Single value)
        {
            _value = value;
        }

        public override void Update(Single bound)
        {
            this.value = (Int32)(bound * _value);
        }
    }
}
