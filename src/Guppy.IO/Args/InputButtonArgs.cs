using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.IO.Utilities;

namespace Guppy.IO.Args
{
    public struct InputButtonArgs
    {
        public readonly InputButton Which;
        public readonly ButtonState State;

        public InputButtonArgs(InputButton which, ButtonState state)
        {
            this.Which = which;
            this.State = state;
        }
    }
}
