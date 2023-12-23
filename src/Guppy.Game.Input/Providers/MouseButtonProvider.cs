using Guppy.Game.Input.Enums;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Providers
{
    internal sealed class MouseButtonProvider : IButtonProvider
    {
        private IButton[] _buttons = Array.Empty<IButton>();

        public void Clean(IEnumerable<IButton> buttons)
        {
            _buttons = buttons.Where(x => x.Source.Type == ButtonType.Mouse).ToArray();
        }

        public IEnumerable<IInput> Update()
        {
            var state = Mouse.GetState();

            foreach (var button in _buttons)
            {
                if (button.Pressed == state.IsButtonDown(button.Source.MouseButton))
                {
                    continue;
                }

                if (button.SetPressed(!button.Pressed, out var message))
                {
                    yield return message;
                }
            }
        }
    }
}
