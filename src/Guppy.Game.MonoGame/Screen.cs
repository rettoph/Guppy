using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame
{
    internal class Screen : IScreen
    {
        public Camera2D Camera { get; }
        public GameWindow Window { get; }

        public GraphicsDevice Graphics { get; }

        public Screen(GraphicsDevice graphics, GameWindow window)
        {
            this.Window = window;
            this.Graphics = graphics;
            this.Camera = new Camera2D(graphics, window)
            {
                Center = false,
                Zoom = 1
            };
        }
    }
}
