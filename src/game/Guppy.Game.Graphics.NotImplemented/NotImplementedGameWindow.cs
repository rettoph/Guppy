using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.NotImplemented
{
    public class NotImplementedGameWindow : IGameWindow
    {
        public GameWindow Value => throw new NotImplementedException();

        public GraphicsObjectStatusEnum Status => GraphicsObjectStatusEnum.NotImplemented;
    }
}
