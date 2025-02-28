﻿using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Services
{
    internal sealed class MouseButtonService : IButtonService
    {
        private IButton[] _buttons = [];

        public void Clean(IEnumerable<IButton> buttons)
        {
            this._buttons = buttons.Where(x => x.Source.Type == ButtonTypeEnum.Mouse).ToArray();
        }

        public IEnumerable<IInputMessage> Update()
        {
            var state = Mouse.GetState();

            foreach (var button in this._buttons)
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