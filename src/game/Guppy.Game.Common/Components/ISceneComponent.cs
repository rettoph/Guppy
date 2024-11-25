using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Attributes;

namespace Guppy.Game.Common.Components
{
    [SceneFilter<IScene>]
    public interface ISceneComponent
    {

    }

    public interface ISceneComponent<TScene> : ISceneComponent
        where TScene : IScene
    {
        [RequireSequenceGroup<InitializeComponentSequenceGroup>]
        void Initialize(TScene scene);
    }
}
