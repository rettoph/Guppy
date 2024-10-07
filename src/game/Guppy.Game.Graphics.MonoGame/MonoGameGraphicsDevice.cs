using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.Graphics.MonoGame
{
    public class MonoGameGraphicsDevice(GraphicsDevice value) : IGraphicsDevice
    {
        public GraphicsDevice Value { get; } = value;
    }
}
