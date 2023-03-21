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
    internal sealed class KeyboardProvider : IInputProvider
    {
        private InputState[] _keys = Array.Empty<InputState>();

        public void Clean(IEnumerable<IInput> inputs)
        {
            _keys = inputs.Where(x => x.Source.Type == Enums.InputType.Keyboard)
                .Select(x => new InputState(x))
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
