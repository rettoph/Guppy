using Guppy.Game.Input.Common.Enums;

namespace Microsoft.Xna.Framework.Input
{
    public static class MouseStateExtensions
    {
        public static bool IsState(this MouseState mouse, CursorButtonsEnum button, ButtonState state)
        {
            return button switch
            {
                CursorButtonsEnum.Left => mouse.LeftButton == state,
                CursorButtonsEnum.Middle => mouse.MiddleButton == state,
                CursorButtonsEnum.Right => mouse.RightButton == state,
                _ => throw new NotImplementedException(),
            };
        }

        public static bool IsButtonUp(this MouseState mouse, CursorButtonsEnum button)
        {
            return mouse.IsState(button, ButtonState.Released);
        }

        public static bool IsButtonDown(this MouseState mouse, CursorButtonsEnum button)
        {
            return mouse.IsState(button, ButtonState.Pressed);
        }
    }
}