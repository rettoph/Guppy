using Guppy.Game.Input.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input
{
    public interface ICursor
    {
        Guid Id { get; }
        Vector2 Position { get; }

        bool this[CursorButtons button] { get; }

        int Scroll { get; }
    }
}
