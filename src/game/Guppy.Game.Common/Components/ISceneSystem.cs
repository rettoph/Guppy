using Guppy.Core.Common.Systems;

namespace Guppy.Game.Common.Systems
{
    public interface ISceneSystem : IScopedSystem
    {

    }

    public interface ISceneSystem<TScene> : ISceneSystem, IInitializeSystem<TScene>
        where TScene : IScene
    {
    }
}