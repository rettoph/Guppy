using Guppy.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Providers
{
    internal sealed class MouseProvider : IInputProvider
    {
        private InputState[] _buttons = Array.Empty<InputState>();

        public void Clean(IEnumerable<IInput> inputs)
        {
            _buttons = inputs.Where(x => x.Source.Type == Enums.InputType.Mouse)
                .Select(x => new InputState(x))
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
