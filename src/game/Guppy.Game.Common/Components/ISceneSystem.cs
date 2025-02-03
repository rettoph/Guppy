using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Game.Common.Systems
{
    public interface ISceneSystem
    {

    }

    public interface ISceneComponent<TScene> : ISceneSystem
        where TScene : IScene
    {
        [RequireSequenceGroup<InitializeSystemSequenceGroupEnum>]
        void Initialize(TScene scene);
    }
}