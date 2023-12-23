using Guppy.Common.Helpers;
using Guppy.Game.Input.Constants;
using Guppy.Game.Input.Enums;
using Guppy.Game.Input.Messages;
using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Guppy.Game.Input
{
    internal sealed class Cursor : ICursor
    {
        private Dictionary<CursorButtons, bool> _buttons;

        public Guid Id { get; } = Cursors.Mouse;

        public Vector2 Position { get; private set; }

        public int Scroll { get; private set; }

        public bool this[CursorButtons button] => _buttons[button];

        public Cursor()
        {
            _buttons = EnumHelper.ToDictionary<CursorButtons, bool>(b => false);
        }

        public bool MoveTo(Vector2 position, [MaybeNullWhen(false)] out CursorMove movement)
        {
            Vector2 delta = position - this.Position;
            if (delta == Vector2.Zero)
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
            if (delta == 0)
            {
                scrolling = null;
                return false;
            }

            this.Scroll = scroll;
            scrolling = new CursorScroll(this, delta);

            return true;
        }

        public bool SetPress(CursorButtons button, bool value, [MaybeNullWhen(false)] out CursorPress press)
        {
            ref bool valueRef = ref CollectionsMarshal.GetValueRefOrNullRef(_buttons, button);

            if (value == valueRef)
            {
                press = null;
                return false;
            }

            valueRef = value;
            press = new CursorPress(this, button, value);

            return true;
        }
    }
}
