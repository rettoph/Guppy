using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common.Components;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Game
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal class DrawGuiComponent : EngineComponent, IGuppyDrawable, IDisposable
    {
        private readonly IGameEngine _engine;
        private readonly IImguiBatch _batch;
        private readonly List<Guppy.Game.Scene> _scenes;
        private IImGuiComponent[] _components;

        public DrawGuiComponent(IImguiBatch batch, IGameEngine engine)
        {
            _batch = batch;
            _engine = engine;
            _components = Array.Empty<IImGuiComponent>();
            _scenes = _engine.Scenes.GetAll().OfType<Guppy.Game.Scene>().ToList();

            _engine.Scenes.OnSceneCreated += this.HandleSceneCreated;
            _engine.Scenes.OnSceneDestroyed += this.HandleSceneDestroyed;
        }

        protected override void Initialize()
        {
            _components = _engine.Components.OfType<IImGuiComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void Dispose()
        {
            _engine.Scenes.OnSceneCreated -= this.HandleSceneCreated;
            _engine.Scenes.OnSceneDestroyed -= this.HandleSceneDestroyed;
        }

        public void Draw(GameTime gameTime)
        {
            _batch.Begin(gameTime);
            foreach (IImGuiComponent component in _components)
            {
                component.DrawImGui(gameTime);
            }

            foreach (Guppy.Game.Scene scene in _scenes)
            {
                scene.DrawGui(gameTime);
            }
            _batch.End();
        }

        private void HandleSceneCreated(ISceneService sender, IScene args)
        {
            if (args is Guppy.Game.Scene scene)
            {
                _scenes.Add(scene);
            }
        }

        private void HandleSceneDestroyed(ISceneService sender, IScene args)
        {
            if (args is Guppy.Game.Scene scene)
            {
                _scenes.Remove(scene);
            }
        }
    }
}
