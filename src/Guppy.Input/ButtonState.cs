using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input
{
    internal sealed class ButtonState
    {
        public readonly IButton Input;
        public bool Pressed;
        public readonly ButtonSource Source;

        public ButtonState(IButton input)
        {
            this.Input = input;
            this.Pressed = false;
            this.Source = this.Input.Source;
        }
    }
}
