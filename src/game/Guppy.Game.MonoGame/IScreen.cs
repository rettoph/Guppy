using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame
{
    public interface IScreen
    {
        Camera2D Camera { get; }
        GameWindow Window { get; }
        GraphicsDevice Graphics { get; }
    }
}
