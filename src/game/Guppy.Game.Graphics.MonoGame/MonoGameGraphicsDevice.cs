using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.Common.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.Graphics.MonoGame
{
    public class MonoGameGraphicsDevice(GraphicsDevice value) : IGraphicsDevice
    {
        public GraphicsDevice Value { get; } = value;
        public GraphicsObjectStatusEnum Status => GraphicsObjectStatusEnum.Implemented;
    }
}