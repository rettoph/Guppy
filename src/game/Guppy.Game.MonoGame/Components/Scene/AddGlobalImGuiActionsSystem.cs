using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Systems;
using Guppy.Game.MonoGame.Services;

namespace Guppy.Game.MonoGame.Components.Scene
{
    public class AddGlobalImGuiActionsSystem(
        IScene scene,
        GlobalImGuiActionService globalImGuiActionService
    ) : ISceneSystem,
        IInitializeSystem,
        IDeinitializeSystem
    {
        private readonly IScene _scene = scene;
        private readonly GlobalImGuiActionService _globalImGuiActionService = globalImGuiActionService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Setup)]
        public void Initialize()
        {
            this._globalImGuiActionService.Add(this._scene.Systems.GetAll());
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.Setup)]
        public void Deinitialize()
        {
            this._globalImGuiActionService.Remove(this._scene.Systems.GetAll());
        }
    }
}
