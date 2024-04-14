using Guppy.Common;

namespace Guppy.Game.Common.Extensions
{
    public static class GuppyEngineExtensions
    {
        public static IGame StartGame(this IGuppyEngine engine)
        {
            return engine.Resolve<IGame>();
        }
    }
}
