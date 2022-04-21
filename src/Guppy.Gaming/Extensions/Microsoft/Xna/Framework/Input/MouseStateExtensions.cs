using Guppy.Gaming.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.Input
{
    public static class MouseStateExtensions
    {
        public static bool IsState(this MouseState mouse, MouseButtons button, ButtonState state)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return mouse.LeftButton == state;
                case MouseButtons.Middle:
                    return mouse.MiddleButton == state;
                case MouseButtons.Right:
                    return mouse.RightButton == state;
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool IsButtonUp(this MouseState mouse, MouseButtons button)
        {
            return mouse.IsState(button, ButtonState.Released);
        }

        public static bool IsButtonDown(this MouseState mouse, MouseButtons button)
        {
            return mouse.IsState(button, ButtonState.Pressed);
        }
    }
}
