using Guppy.Input.Constants;
using Guppy.Input.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input
{
    internal sealed class MouseCursor : ICursor
    {
        public Guid Id { get; } = Cursors.Mouse;

        public Vector2 Position { get; private set; }

        public bool MoveTo(Vector2 position, [MaybeNullWhen(false)] out CursorMove movement)
        {
            Vector2 delta = position - this.Position;
            this.Position = position;
            movement = new CursorMove(this, delta);

            return delta != Vector2.Zero;
        }
    }
}
