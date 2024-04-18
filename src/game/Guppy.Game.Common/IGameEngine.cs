using Guppy.Engine.Common;
using Guppy.Game.Common.Services;

namespace Guppy.Game.Common
{
    public interface IGameEngine : IGuppyEngine, IGuppyDrawable, IGuppyUpdateable
    {
        ISceneService Scenes { get; }

        new IGameEngine Start();
    }
}
