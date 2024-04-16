using Guppy.Game.Input.Common.Enums;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game.Input.Common.Messages
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
