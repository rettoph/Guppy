using Guppy.Common;
using Guppy.Messaging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Messages
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
