using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Guppy.Core.Common.Helpers;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Constants;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.Input.Common.Messages;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input
{
    internal sealed class MouseCursor : ICursor
    {
        private readonly Dictionary<CursorButtonsEnum, bool> _buttons;

        public Guid Id { get; } = Cursors.Mouse;

        public Vector2 Position { get; private set; }

        public int Scroll { get; private set; }

        public bool this[CursorButtonsEnum button] => this._buttons[button];

        public MouseCursor()
        {
            this._buttons = EnumHelper.ToDictionary<CursorButtonsEnum, bool>(b => false);
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

        public bool SetPress(CursorButtonsEnum button, bool value, [MaybeNullWhen(false)] out CursorPress press)
        {
            ref bool valueRef = ref CollectionsMarshal.GetValueRefOrNullRef(this._buttons, button);

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