using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public interface IInput
    {
        string Key { get; }
        InputSource DefaultSource { get; }
        InputSource Source { get; set; }
        ButtonState State { get; }

        public event OnChangedEventDelegate<IInput, InputSource>? OnSourceChanged;
        public event OnChangedEventDelegate<IInput, ButtonState>? OnStateChanged;

        void Update(ref KeyboardState kState, ref MouseState mState);
    }
}
