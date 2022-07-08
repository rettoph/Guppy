using Guppy.MonoGame.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputSource
    {
        [FieldOffset(0)]
        public readonly InputType Type;

        [FieldOffset(1)]
        public readonly Keys KeyboardKey;

        [FieldOffset(1)]
        public readonly MouseButtons MouseButton;


        public InputSource(Keys keyboardKey)
        {
            this.Type = InputType.Keyboard;
            this.MouseButton = default(MouseButtons);
            this.KeyboardKey = keyboardKey;
        }

        public InputSource(MouseButtons mouseButtons)
        {
            this.Type = InputType.Mouse;
            this.KeyboardKey = default(Keys);
            this.MouseButton = mouseButtons;
        }

        public override string ToString()
        {
            switch (this.Type)
            {
                case InputType.Mouse:
                    return this.MouseButton.ToString();
                case InputType.Keyboard:
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
            return obj is InputSource source &&
                   Type == source.Type &&
                   KeyboardKey == source.KeyboardKey;
        }

        public static implicit operator InputSource(MouseButtons mouseButtons)
        {
            return new InputSource(mouseButtons);
        }

        public static implicit operator InputSource(Keys keyboardKey)
        {
            return new InputSource(keyboardKey);
        }
        public static bool operator ==(InputSource a, InputSource b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(InputSource a, InputSource b)
        {
            return !(a == b);
        }
    }
}
