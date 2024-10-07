using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.NotImplemented
{
    public class NotImplementedGameWindow : IGameWindow
    {
        public GameWindow Value => throw new NotImplementedException();
    }
}
