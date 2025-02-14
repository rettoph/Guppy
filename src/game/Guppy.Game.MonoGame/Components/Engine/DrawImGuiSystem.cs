using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.MonoGame.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Systems.Engine
{
    public class DrawImGuiSystem(
        IGameEngine engine,
        IImguiBatch batch,
        GlobalImGuiActionService globalImGuiActionService
    ) : IEngineSystem,
        IInitializeSystem,
        IDeinitializeSystem,
        IDrawSystem
    {
        private readonly IGameEngine _engine = engine;
        private readonly IImguiBatch _batch = batch;
        private readonly GlobalImGuiActionService _globalImGuiActionService = globalImGuiActionService;


        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Initialize)]
        public void Initialize()
        {
            this._globalImGuiActionService.Add(this._engine.Systems.GetAll());
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.Initialize)]
        public void Deinitialize()
        {
            this._globalImGuiActionService.Remove(this._engine.Systems.GetAll());
        }

        [SequenceGroup<DrawSequenceGroupEnum>(DrawSequenceGroupEnum.Draw)]
        public void Draw(GameTime gameTime)
        {
            this._globalImGuiActionService.Draw(gameTime);
        }

        [SequenceGroup<ImGuiSequenceGroupEnum>(ImGuiSequenceGroupEnum.Begin)]
        public void BeginImgui(GameTime gameTime)
        {
            this._batch.Begin(gameTime);
        }

        [SequenceGroup<ImGuiSequenceGroupEnum>(ImGuiSequenceGroupEnum.EndDraw)]
#pragma warning disable IDE0060 // Remove unused parameter
        public void EndImGui(GameTime gameTime)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            this._batch.End();
        }
    }
}