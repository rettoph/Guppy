using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.IO.Utilities;
using Guppy.IO.EventArgs;
using Guppy.Threading.Interfaces;
using Guppy.IO.Structs;

namespace Guppy.IO.EventArgs
{
    public class InputButtonArgs : System.EventArgs, IMessage
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
