using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.MonoGame
{
    public class MonoGameGameWindow(GameWindow value) : IGameWindow
    {
        public GameWindow Value { get; } = value;
    }
}
