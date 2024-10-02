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
    [AutoLoad]
    internal class DrawImGuiComponent : IEngineComponent, IDrawableComponent, IDisposable
    {
        private readonly IGameEngine _engine;
        private readonly IImguiBatch _batch;
        private readonly ActionSequenceGroup<ImGuiSequenceGroup, GameTime> _imguiActions;

        public DrawImGuiComponent(IImguiBatch batch, IGameEngine engine)
        {
            _batch = batch;
            _engine = engine;
            _imguiActions = new ActionSequenceGroup<ImGuiSequenceGroup, GameTime>();
            _engine.Scenes.OnSceneCreated += this.HandleSceneCreated;
            _engine.Scenes.OnSceneDestroyed += this.HandleSceneDestroyed;
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            _imguiActions.Add(_engine.Components);
        }

        public void Dispose()
        {
            _engine.Scenes.OnSceneCreated -= this.HandleSceneCreated;
            _engine.Scenes.OnSceneDestroyed -= this.HandleSceneDestroyed;
        }

        [SequenceGroup<DrawComponentSequenceGroup>(DrawComponentSequenceGroup.Draw)]
        public void Draw(GameTime gameTime)
        {
            _imguiActions.Invoke(gameTime);
        }

        [SequenceGroup<ImGuiSequenceGroup>(ImGuiSequenceGroup.Begin)]
        public void BeginImgui(GameTime gameTime)
        {
            _batch.Begin(gameTime);
        }

        [SequenceGroup<ImGuiSequenceGroup>(ImGuiSequenceGroup.EndDraw)]
        public void EndImGui(GameTime gameTime)
        {
            _batch.End();
        }

        private void HandleSceneCreated(ISceneService sender, IScene scene)
        {
            _imguiActions.Add(scene.Components);
        }

        private void HandleSceneDestroyed(ISceneService sender, IScene scene)
        {
            _imguiActions.Remove(scene.Components);

            // I dont think ActionSequenceGroup.Remove works as expected. 
            // TODO: Test
            throw new NotImplementedException();
        }
    }
}
