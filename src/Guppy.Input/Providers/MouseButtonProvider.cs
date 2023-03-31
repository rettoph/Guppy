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
        private IButton[] _buttons = Array.Empty<IButton>();

        public void Clean(IEnumerable<IButton> buttons)
        {
            _buttons = buttons.Where(x => x.Source.Type == ButtonType.Mouse).ToArray();
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

                if (button.SetPressed(!button.Pressed, out var message))
                {
                    yield return message;
                }
            }
        }
    }
}
