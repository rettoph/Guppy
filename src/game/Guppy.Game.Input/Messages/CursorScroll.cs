using Guppy.Core.Messaging;

namespace Guppy.Game.Input.Messages
{
    public sealed class CursorScroll : Message<CursorScroll>, IInput
    {
        public readonly ICursor Cursor;
        public readonly int Delta;
        public readonly float Ratio;

        public CursorScroll(ICursor cursor, int delta)
        {
            this.Cursor = cursor;
            this.Delta = delta;
            this.Ratio = delta / 120f;
        }
    }
}
