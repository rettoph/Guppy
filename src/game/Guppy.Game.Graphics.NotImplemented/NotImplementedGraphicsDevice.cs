using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.Common.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.Graphics.NotImplemented
{
    public class NotImplementedGraphicsDevice : IGraphicsDevice
    {
        public GraphicsDevice Value => throw new NotImplementedException();

        public GraphicsObjectStatusEnum Status => GraphicsObjectStatusEnum.NotImplemented;
    }
}
