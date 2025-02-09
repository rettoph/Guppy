using Guppy.Game.Input.Common.Enums;

namespace Guppy.Game.Input.Common.Messages
{
    public class CursorPress(ICursor cursor, CursorButtonsEnum button, bool value) : InputMessage<CursorPress>, IInputMessage
    {
        public readonly ICursor Cursor = cursor;
        public readonly CursorButtonsEnum Button = button;
        public readonly bool Value = value;
    }
}