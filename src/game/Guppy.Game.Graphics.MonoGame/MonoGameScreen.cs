using Guppy.Game.Graphics.Common;
using Guppy.Game.MonoGame.Common.Utilities.Cameras;

namespace Guppy.Game.Graphics.MonoGame
{
    public class MonoGameScreen(IGraphicsDevice graphics, IGameWindow window) : IScreen
    {
        public ICamera2D Camera => new MonoGameCamera2D(graphics, window)
        {
            Center = false,
            Zoom = 1
        };
    }
}
