using Microsoft.Xna.Framework;

namespace Guppy.Game.Input.Common.Messages
{
    public sealed class CursorMove(ICursor cursor, Vector2 delta) : InputMessage<CursorMove>, IInputMessage
    {
        public readonly ICursor Cursor = cursor;
        public readonly Vector2 Delta = delta;
    }
}