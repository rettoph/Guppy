using Guppy.Input.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ButtonSource
    {
        [FieldOffset(0)]
        public readonly ButtonType Type;

        [FieldOffset(1)]
        public readonly Keys KeyboardKey;

        [FieldOffset(1)]
        public readonly CursorButtons MouseButton;


        public ButtonSource(Keys keyboardKey)
        {
            this.Type = ButtonType.Keyboard;
            this.MouseButton = default(CursorButtons);
            this.KeyboardKey = keyboardKey;
        }

        public ButtonSource(CursorButtons mouseButtons)
        {
            this.Type = ButtonType.Mouse;
            this.KeyboardKey = default(Keys);
            this.MouseButton = mouseButtons;
        }

        public override string ToString()
        {
            switch (this.Type)
            {
                case ButtonType.Mouse:
                    return this.MouseButton.ToString();
                case ButtonType.Keyboard:
                    return this.KeyboardKey.ToString();
                default:
                    throw new NotImplementedException();
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, KeyboardKey);
        }

        public override bool Equals(object? obj)
        {
            return obj is ButtonSource source &&
                   Type == source.Type &&
                   KeyboardKey == source.KeyboardKey;
        }

        public static implicit operator ButtonSource(CursorButtons mouseButtons)
        {
            return new ButtonSource(mouseButtons);
        }

        public static implicit operator ButtonSource(Keys keyboardKey)
        {
            return new ButtonSource(keyboardKey);
        }
        public static bool operator ==(ButtonSource a, ButtonSource b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ButtonSource a, ButtonSource b)
        {
            return !(a == b);
        }
    }
}
