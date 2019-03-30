using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Structs
{
    public struct MouseData
    {
        public Vector2 Delta;
        public Vector2 Position;
        public ButtonState LeftButton;
        public Int32 ScrollDelta;
    }
}
