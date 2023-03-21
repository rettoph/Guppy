using Guppy.Common;
using Guppy.Input.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Providers
{
    internal sealed class MouseButtonProvider : IButtonProvider
    {
        private ButtonState[] _buttons = Array.Empty<ButtonState>();

        public void Clean(IEnumerable<IButton> buttons)
        {
            _buttons = buttons.Where(x => x.Source.Type == ButtonType.Mouse)
                .Select(x => new ButtonState(x))
                .ToArray();
        }

        public IEnumerable<IMessage> Update()
        {
            var state = Mouse.GetState();

            foreach(var button in _buttons)
            {
                if(button.Pressed == state.IsButtonDown(button.Source.MouseButton))
                {
                    continue;
                }

                button.Pressed = !button.Pressed;
                if (button.Input.Message(button.Pressed, out var message))
                {
                    yield return message;
                }
            }
        }
    }
}
