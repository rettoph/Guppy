using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Input
{
    public struct ScrollWheelArgs
    {
        public readonly Single Value;
        public readonly Single Delta;

        internal ScrollWheelArgs(float value, float delta)
        {
            this.Value = value;
            this.Delta = delta;
        }
    }
}
