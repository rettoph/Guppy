using Guppy.Game.Graphics.Common;

namespace Guppy.Game.Graphics.NotImplemented
{
    public class NotImplementedScreen : IScreen
    {
        public ICamera2D Camera => throw new NotImplementedException();
    }
}