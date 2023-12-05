using Guppy.Common;
using Guppy.Game.Input.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.Input.Providers
{
    internal sealed class KeyboardButtonProvider : IButtonProvider
    {
        private IButton[] _keys = Array.Empty<IButton>();

        public void Clean(IEnumerable<IButton> buttons)
        {
            _keys = buttons.Where(x => x.Source.Type == ButtonType.Keyboard).ToArray();
        }

        public IEnumerable<IInput> Update()
        {
            var state = Keyboard.GetState();

            foreach(var key in _keys)
            {
                if(key.Pressed == state.IsKeyDown(key.Source.KeyboardKey))
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
