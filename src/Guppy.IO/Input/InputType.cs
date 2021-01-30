using Guppy.IO.Enums;
using Guppy.IO.Input.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Guppy.IO.Input
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputType
    {
        [FieldOffset(0)]
        public readonly InputTypeType Type;

        [FieldOffset(1)]
        public readonly Keys KeyboardKey;

        [FieldOffset(1)]
        public readonly MouseButton MouseButton;


        public InputType(Keys keyboardKey)
        {
            this.Type = InputTypeType.Keyboard;
            this.MouseButton = default(MouseButton);
            this.KeyboardKey = keyboardKey;
        }

        public InputType(MouseButton cursorButton)
        {
            this.Type = InputTypeType.MouseButton;
            this.KeyboardKey = default(Keys);
            this.MouseButton = cursorButton;
        }

        #region ToString Methods
        public override string ToString()
        {
            switch (this.Type)
            {
                case InputTypeType.MouseButton:
                    return this.MouseButton.ToString();
                case InputTypeType.Keyboard:
                    return this.KeyboardKey.ToString();
                default:
                    return base.ToString();
            }
        }
        #endregion
    }
}
