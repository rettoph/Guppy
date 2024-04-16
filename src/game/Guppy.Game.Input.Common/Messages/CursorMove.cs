using Guppy.Core.Messaging.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input.Common.Messages
{
    public sealed class CursorMove : Message<CursorMove>, IInput
    {
        public readonly ICursor Cursor;
        public readonly Vector2 Delta;

        public CursorMove(ICursor cursor, Vector2 delta)
        {
            this.Cursor = cursor;
            this.Delta = delta;
        }
    }
}
