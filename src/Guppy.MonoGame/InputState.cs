using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal sealed class InputState
    {
        public readonly IInput Input;
        public bool Pressed;
        public readonly InputSource Source;

        public InputState(IInput input)
        {
            this.Input = input;
            this.Pressed = false;
            this.Source = this.Input.Source;
        }
    }
}
