using Guppy.Game.Input.Common.Enums;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game.Input.Common.Messages
{
    public class CursorPress(ICursor cursor, CursorButtons button, bool value) : Message<CursorPress>, IInput
    {
        public readonly ICursor Cursor = cursor;
        public readonly CursorButtons Button = button;
        public readonly bool Value = value;
    }
}
