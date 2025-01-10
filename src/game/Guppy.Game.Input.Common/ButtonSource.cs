using System.Runtime.InteropServices;
using Guppy.Game.Input.Common.Enums;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Common
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct ButtonSource
    {
        [FieldOffset(0)]
        public readonly ButtonTypeEnum Type;

        [FieldOffset(1)]
        public readonly Keys KeyboardKey;

        [FieldOffset(1)]
        public readonly CursorButtonsEnum MouseButton;


        public ButtonSource(Keys keyboardKey)
        {
            this.Type = ButtonTypeEnum.Keyboard;
            this.MouseButton = default;
            this.KeyboardKey = keyboardKey;
        }

        public ButtonSource(CursorButtonsEnum mouseButtons)
        {
            this.Type = ButtonTypeEnum.Mouse;
            this.KeyboardKey = default;
            this.MouseButton = mouseButtons;
        }

        public override readonly string ToString() => this.Type switch
        {
            ButtonTypeEnum.Mouse => this.MouseButton.ToString(),
            ButtonTypeEnum.Keyboard => this.KeyboardKey.ToString(),
            _ => throw new NotImplementedException(),
        };

        public override readonly int GetHashCode() => HashCode.Combine(this.Type, this.KeyboardKey);

        public override readonly bool Equals(object? obj) => obj is ButtonSource source &&
                   this.Type == source.Type &&
                   this.KeyboardKey == source.KeyboardKey;

        public static implicit operator ButtonSource(CursorButtonsEnum mouseButtons)
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