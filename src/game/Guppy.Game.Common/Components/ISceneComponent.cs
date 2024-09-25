using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Attributes;

namespace Guppy.Game.Common.Components
{
    [SceneFilter<IScene>]
    [Service(ServiceLifetime.Scoped, ServiceRegistrationFlags.RequireAutoLoadAttribute | ServiceRegistrationFlags.AsImplementedInterfaces)]
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
