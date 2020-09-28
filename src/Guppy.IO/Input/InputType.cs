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
        public readonly MouseButton CursorButton;


        public InputType(Keys keyboardKey)
        {
            this.Type = InputTypeType.Keyboard;
            this.CursorButton = default(MouseButton);
            this.KeyboardKey = keyboardKey;
        }

        public InputType(MouseButton cursorButton)
        {
            this.Type = InputTypeType.MouseButton;
            this.KeyboardKey = default(Keys);
            this.CursorButton = cursorButton;
        }
    }
}
