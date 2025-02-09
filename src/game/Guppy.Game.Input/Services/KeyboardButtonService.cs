using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Services
{
    internal sealed class KeyboardButtonService : IButtonService
    {
        private IButton[] _keys = [];

        public void Clean(IEnumerable<IButton> buttons)
        {
            this._keys = buttons.Where(x => x.Source.Type == ButtonTypeEnum.Keyboard).ToArray();
        }

        public IEnumerable<IInputMessage> Update()
        {
            var state = Keyboard.GetState();

            foreach (var key in this._keys)
            {
                if (key.Pressed == state.IsKeyDown(key.Source.KeyboardKey))
                {
                    continue;
                }

                if (key.SetPressed(!key.Pressed, out var message))
                {
                    yield return message;
                }
            }
        }
    }
}