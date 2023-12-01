using Guppy.Common;
using Guppy.Input.Enums;
using Guppy.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Messages
{
    public class CursorPress : Message<CursorPress>, IInput
    {
        public readonly ICursor Cursor;
        public readonly CursorButtons Button;
        public readonly bool Value;

        public CursorPress(ICursor cursor, CursorButtons button, bool value)
        {
            this.Cursor = cursor;
            this.Button = button;
            this.Value = value;
        }
    }
}
