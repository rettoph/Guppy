using Guppy.IO.Enums;
using Guppy.Threading.Interfaces;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Guppy.IO.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputButton
    {
        [FieldOffset(0)]
        public readonly InputButtonType Type;

        [FieldOffset(1)]
        public readonly Keys KeyboardKey;

        [FieldOffset(1)]
        public readonly MouseButton MouseButton;

        public InputButton(Keys keyboardKey)
        {
            this.Type = InputButtonType.Keyboard;
            this.MouseButton = default(MouseButton);
            this.KeyboardKey = keyboardKey;
        }

        public InputButton(MouseButton mouseButton)
        {
            this.Type = InputButtonType.MouseButton;
            this.KeyboardKey = default(Keys);
            this.MouseButton = mouseButton;
        }

        #region ToString Methods
        public override string ToString()
        {
            switch (this.Type)
            {
                case InputButtonType.MouseButton:
                    return this.MouseButton.ToString();
                case InputButtonType.Keyboard:
                    return this.KeyboardKey.ToString();
                default:
                    return base.ToString();
            }
        }
        #endregion
    }
}
