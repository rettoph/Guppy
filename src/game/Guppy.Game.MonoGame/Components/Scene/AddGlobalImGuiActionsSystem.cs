using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Systems;
using Guppy.Game.MonoGame.Services;

namespace Guppy.Game.MonoGame.Components.Scene
{
    public class AddGlobalImGuiActionsSystem(
        GlobalImGuiActionService globalImGuiActionService
    ) : ISceneSystem,
        IInitializeSystem<IScene>,
        IDeinitializeSystem<IScene>
    {
        private readonly GlobalImGuiActionService _globalImGuiActionService = globalImGuiActionService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Setup)]
        public void Initialize(IScene scene)
        {
            this._globalImGuiActionService.Add(scene.Systems);
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.Setup)]
        public void Deinitialize(IScene scene)
        {
            this._globalImGuiActionService.Remove(scene.Systems);
        }
    }
}
