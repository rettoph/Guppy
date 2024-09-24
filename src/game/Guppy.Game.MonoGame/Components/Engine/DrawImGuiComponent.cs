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
    internal class DrawImGuiComponent : IEngineComponent, IInitializableComponent, IDrawableComponent, IDisposable
    {
        private readonly IGameEngine _engine;
        private readonly IImguiBatch _batch;
        private ActionSequenceGroup<DrawImGuiSequenceGroup, GameTime> _drawImGuiActions;

        public DrawImGuiComponent(IImguiBatch batch, IGameEngine engine)
        {
            _batch = batch;
            _engine = engine;
            _drawImGuiActions = new ActionSequenceGroup<DrawImGuiSequenceGroup, GameTime>();
            _engine.Scenes.OnSceneCreated += this.HandleSceneCreated;
            _engine.Scenes.OnSceneDestroyed += this.HandleSceneDestroyed;
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            _drawImGuiActions.Add(_engine.Components);
        }

        public void Dispose()
        {
            _engine.Scenes.OnSceneCreated -= this.HandleSceneCreated;
            _engine.Scenes.OnSceneDestroyed -= this.HandleSceneDestroyed;
        }

        [SequenceGroup<DrawComponentSequenceGroup>(DrawComponentSequenceGroup.Draw)]
        public void Draw(GameTime gameTime)
        {
            _drawImGuiActions.Invoke(gameTime);
        }

        [SequenceGroup<DrawImGuiSequenceGroup>(DrawImGuiSequenceGroup.BeginDraw)]
        public void BeginImgui(GameTime gameTime)
        {
            _batch.Begin(gameTime);
        }

        [SequenceGroup<DrawImGuiSequenceGroup>(DrawImGuiSequenceGroup.EndDraw)]
        public void EndImGui(GameTime gameTime)
        {
            _batch.End();
        }

        private void HandleSceneCreated(ISceneService sender, IScene scene)
        {
            scene.OnVisibleChanged += this.HandleSceneVisibleChanged;

            if (scene.Visible == true)
            {
                this.HandleSceneVisibleChanged(scene, true);
            }
        }

        private void HandleSceneDestroyed(ISceneService sender, IScene scene)
        {
            scene.OnVisibleChanged -= this.HandleSceneVisibleChanged;

            this.HandleSceneVisibleChanged(scene, false);
        }

        private void HandleSceneVisibleChanged(IScene scene, bool visible)
        {
            if (visible == true)
            {
                _drawImGuiActions.Add(scene.Components);
                return;
            }

            if (visible == false)
            {
                _drawImGuiActions.Remove(scene.Components);
                return;
            }
        }
    }
}
