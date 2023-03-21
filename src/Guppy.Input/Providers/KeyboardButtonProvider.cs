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
    internal sealed class KeyboardButtonProvider : IButtonProvider
    {
        private ButtonState[] _keys = Array.Empty<ButtonState>();

        public void Clean(IEnumerable<IButton> buttons)
        {
            _keys = buttons.Where(x => x.Source.Type == ButtonType.Keyboard)
                .Select(x => new ButtonState(x))
                .ToArray();
        }

        public IEnumerable<IMessage> Update()
        {
            var state = Keyboard.GetState();

            foreach(var key in _keys)
            {
                if(key.Pressed == state.IsKeyDown(key.Source.KeyboardKey))
                {
                    continue;
                }

                key.Pressed = !key.Pressed;
                if (key.Input.Message(key.Pressed, out var message))
                {
                    yield return message;
                }
            }
        }
    }
}
