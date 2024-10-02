﻿using Guppy.Game.Input.Common;
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
            _keys = buttons.Where(x => x.Source.Type == ButtonType.Keyboard).ToArray();
        }

        public IEnumerable<IInput> Update()
        {
            var state = Keyboard.GetState();

            foreach (var key in _keys)
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
