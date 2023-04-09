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

        public int Scroll { get; private set; }

        public bool MoveTo(Vector2 position, [MaybeNullWhen(false)] out CursorMove movement)
        {
            Vector2 delta = position - this.Position;
            if(delta == Vector2.Zero)
            {
                movement = null;
                return false;
            }

            this.Position = position;
            movement = new CursorMove(this, delta);

            return true;
        }

        public bool ScrollTo(int scroll, [MaybeNullWhen(false)] out CursorScroll scrolling)
        {
            int delta = scroll - this.Scroll;
            if(delta == 0 )
            {
                scrolling = null;
                return false;
            }

            this.Scroll = scroll;
            scrolling = new CursorScroll(this, delta);

            return true;
        }
    }
}
