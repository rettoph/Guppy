using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Engine.Common.Systems;

namespace Guppy.Game.ImGui.MonoGame.Systems.Engine
{
    public class InitializeImGuiBatchSystem(MonoGameImGuiBatch imGuiBatch) : IEngineSystem, IInitializeSystem
    {
        private readonly MonoGameImGuiBatch _imGuiBatch = imGuiBatch;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PostInitialize)]
        public void Initialize()
        {
            this._imGuiBatch.Initialize();
        }
    }
}
