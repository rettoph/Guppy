using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Guppy.Game.Common.Systems;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Systems.Engine
{
    public class DrawImGuiSystem : IEngineSystem, IDrawableSystem, IDisposable
    {
        private readonly IGameEngine _engine;
        private readonly IImguiBatch _batch;
        private readonly ActionSequenceGroup<ImGuiSequenceGroupEnum, GameTime> _imguiActions;

        public DrawImGuiSystem(IImguiBatch batch, IGameEngine engine)
        {
            this._batch = batch;
            this._engine = engine;
            this._imguiActions = new ActionSequenceGroup<ImGuiSequenceGroupEnum, GameTime>(true);
        }

        [SequenceGroup<InitializeSystemSequenceGroupEnum>(InitializeSystemSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            this._imguiActions.Add(this._engine.Systems);
            this._engine.Scenes.OnSceneCreated += this.HandleSceneCreated;
            this._engine.Scenes.OnSceneDestroyed += this.HandleSceneDestroyed;
        }

        public void Dispose()
        {
            this._engine.Scenes.OnSceneCreated -= this.HandleSceneCreated;
            this._engine.Scenes.OnSceneDestroyed -= this.HandleSceneDestroyed;
        }

        [SequenceGroup<DrawComponentSequenceGroupEnum>(DrawComponentSequenceGroupEnum.Draw)]
        public void Draw(GameTime gameTime)
        {
            this._imguiActions.Invoke(gameTime);
        }

        [SequenceGroup<ImGuiSequenceGroupEnum>(ImGuiSequenceGroupEnum.Begin)]
        public void BeginImgui(GameTime gameTime)
        {
            this._batch.Begin(gameTime);
        }

        [SequenceGroup<ImGuiSequenceGroupEnum>(ImGuiSequenceGroupEnum.EndDraw)]
#pragma warning disable IDE0060 // Remove unused parameter
        public void EndImGui(GameTime gameTime)
        {
            this._batch.End();
        }

        private void HandleSceneCreated(ISceneService sender, IScene scene)
        {
            this._imguiActions.Add(scene.Systems);
        }

        private void HandleSceneDestroyed(ISceneService sender, IScene scene)
        {
            this._imguiActions.Remove(scene.Systems);

            // I dont think ActionSequenceGroup.Remove works as expected. 
            // TODO: Test
            throw new NotImplementedException();
        }
    }
}