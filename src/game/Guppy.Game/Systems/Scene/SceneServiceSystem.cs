using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Systems;
using Guppy.Game.Services;

namespace Guppy.Game.Systems.Scene
{
    public class SceneServiceSystem(
        SceneService sceneService
    ) : ISceneSystem,
        IInitializeSystem<IScene>,
        IDeinitializeSystem<IScene>
    {
        private readonly SceneService _sceneService = sceneService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Cleanup)]
        public void Initialize(IScene scene)
        {
            this._sceneService.Add(scene);
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.Cleanup)]
        public void Deinitialize(IScene scene)
        {
            this._sceneService.Remove(scene);
        }
    }
}
