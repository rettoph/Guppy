using Guppy.Game.Input.Common.Enums;

namespace Microsoft.Xna.Framework.Input
{
    public static class MouseStateExtensions
    {
        public static bool IsState(this MouseState mouse, CursorButtons button, ButtonState state)
        {
            switch (button)
            {
                case CursorButtons.Left:
                    return mouse.LeftButton == state;
                case CursorButtons.Middle:
                    return mouse.MiddleButton == state;
                case CursorButtons.Right:
                    return mouse.RightButton == state;
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool IsButtonUp(this MouseState mouse, CursorButtons button)
        {
            return mouse.IsState(button, ButtonState.Released);
        }

        public static bool IsButtonDown(this MouseState mouse, CursorButtons button)
        {
            return mouse.IsState(button, ButtonState.Pressed);
        }
    }
}
