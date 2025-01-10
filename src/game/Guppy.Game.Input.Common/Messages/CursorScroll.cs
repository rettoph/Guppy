using Guppy.Core.Messaging.Common;

namespace Guppy.Game.Input.Common.Messages
{
    public sealed class CursorScroll(ICursor cursor, int delta) : Message<CursorScroll>, IInput
    {
        public readonly ICursor Cursor = cursor;
        public readonly int Delta = delta;
        public readonly float Ratio = delta / 120f;
    }
}