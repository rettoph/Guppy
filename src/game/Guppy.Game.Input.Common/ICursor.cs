using System.Diagnostics.CodeAnalysis;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.Input.Common.Messages;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input.Common
{
    public interface ICursor
    {
        Guid Id { get; }
        Vector2 Position { get; }

        bool this[CursorButtonsEnum button] { get; }

        int Scroll { get; }

        bool MoveTo(Vector2 position, [MaybeNullWhen(false)] out CursorMove movement);

        bool ScrollTo(int scroll, [MaybeNullWhen(false)] out CursorScroll scrolling);

        bool SetPress(CursorButtonsEnum button, bool value, [MaybeNullWhen(false)] out CursorPress press);
    }
}