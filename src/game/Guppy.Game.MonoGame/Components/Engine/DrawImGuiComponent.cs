using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Engine
{
    internal class DrawImGuiComponent : IEngineComponent, IDrawableComponent, IDisposable
    {
        private readonly IGameEngine _engine;
        private readonly IImguiBatch _batch;
        private readonly ActionSequenceGroup<ImGuiSequenceGroupEnum, GameTime> _imguiActions;

        public DrawImGuiComponent(IImguiBatch batch, IGameEngine engine)
        {
            this._batch = batch;
            this._engine = engine;
            this._imguiActions = new ActionSequenceGroup<ImGuiSequenceGroupEnum, GameTime>(true);
            this._engine.Scenes.OnSceneCreated += this.HandleSceneCreated;
            this._engine.Scenes.OnSceneDestroyed += this.HandleSceneDestroyed;
        }

        [SequenceGroup<InitializeComponentSequenceGroupEnum>(InitializeComponentSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            this._imguiActions.Add(this._engine.Components);
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
            this._imguiActions.Add(scene.Components);
        }

        private void HandleSceneDestroyed(ISceneService sender, IScene scene)
        {
            this._imguiActions.Remove(scene.Components);

            // I dont think ActionSequenceGroup.Remove works as expected. 
            // TODO: Test
            throw new NotImplementedException();
        }
    }
}