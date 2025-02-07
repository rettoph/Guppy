using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Systems;
using Guppy.Game.Services;

namespace Guppy.Game.Systems.Scene
{
    public class SceneServiceSystem(
        IScene scene,
        SceneService sceneService
    ) : ISceneSystem,
        IInitializeSystem,
        IDeinitializeSystem
    {
        private readonly IScene _scene = scene;
        private readonly SceneService _sceneService = sceneService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Cleanup)]
        public void Initialize()
        {
            this._sceneService.Add(this._scene);
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.Cleanup)]
        public void Deinitialize()
        {
            this._sceneService.Remove(this._scene);
        }
    }
}
