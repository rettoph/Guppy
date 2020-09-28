using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Input
{
    public struct InputArgs
    {
        public readonly InputType Which;
        public readonly ButtonState State;

        public InputArgs(InputType which, ButtonState state)
        {
            this.Which = which;
            this.State = state;
        }
    }
}
