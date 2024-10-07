using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.Graphics.NotImplemented
{
    public class NotImplementedGraphicsDevice : IGraphicsDevice
    {
        public GraphicsDevice Value => throw new NotImplementedException();
    }
}
