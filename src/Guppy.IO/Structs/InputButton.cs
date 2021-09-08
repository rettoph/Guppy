using Guppy.IO.Enums;
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
        public readonly Enums.InputButtonType Type;

        [FieldOffset(1)]
        public readonly Keys KeyboardKey;

        [FieldOffset(1)]
        public readonly MouseButton MouseButton;


        public InputButton(Keys keyboardKey)
        {
            this.Type = Enums.InputButtonType.Keyboard;
            this.MouseButton = default(MouseButton);
            this.KeyboardKey = keyboardKey;
        }

        public InputButton(MouseButton cursorButton)
        {
            this.Type = Enums.InputButtonType.MouseButton;
            this.KeyboardKey = default(Keys);
            this.MouseButton = cursorButton;
        }

        #region ToString Methods
        public override string ToString()
        {
            switch (this.Type)
            {
                case Enums.InputButtonType.MouseButton:
                    return this.MouseButton.ToString();
                case Enums.InputButtonType.Keyboard:
                    return this.KeyboardKey.ToString();
                default:
                    return base.ToString();
            }
        }
        #endregion
    }
}
