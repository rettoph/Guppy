namespace Microsoft.Xna.Framework.Input
{
    public static class KeyboardStateExtensions
    {
        public static bool IsState(this KeyboardState keyboard, Keys key, ButtonState state) => state switch
        {
            ButtonState.Pressed => keyboard.IsKeyDown(key),
            ButtonState.Released => keyboard.IsKeyUp(key),
            _ => throw new NotImplementedException(),
        };
    }
}