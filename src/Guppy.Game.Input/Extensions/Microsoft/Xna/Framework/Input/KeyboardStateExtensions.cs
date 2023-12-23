namespace Microsoft.Xna.Framework.Input
{
    public static class KeyboardStateExtensions
    {
        public static bool IsState(this KeyboardState keyboard, Keys key, ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    return keyboard.IsKeyDown(key);
                case ButtonState.Released:
                    return keyboard.IsKeyUp(key);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
