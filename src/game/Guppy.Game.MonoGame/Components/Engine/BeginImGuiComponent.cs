using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Engine
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    [Sequence<DrawSequence>(DrawSequence.PreDraw)]
    internal class BeginImGuiComponent : EngineComponent, IGuppyDrawable, IDisposable
    {
        private readonly IGameEngine _engine;
        private readonly IImguiBatch _batch;
        private readonly List<Game.Common.Scene> _scenes;
        private IImGuiComponent[] _components;

        public BeginImGuiComponent(IImguiBatch batch, IGameEngine engine)
        {
            _batch = batch;
            _engine = engine;
            _components = Array.Empty<IImGuiComponent>();
            _scenes = _engine.Scenes.GetAll().OfType<Game.Common.Scene>().ToList();

            _engine.Scenes.OnSceneCreated += this.HandleSceneCreated;
            _engine.Scenes.OnSceneDestroyed += this.HandleSceneDestroyed;
        }

        protected override void Initialize()
        {
            _components = _engine.Components.Sequence<IImGuiComponent, DrawSequence>().ToArray();
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
        }

        private void HandleSceneCreated(ISceneService sender, IScene args)
        {
            if (args is Game.Common.Scene scene)
            {
                _scenes.Add(scene);
            }
        }

        private void HandleSceneDestroyed(ISceneService sender, IScene args)
        {
            if (args is Game.Common.Scene scene)
            {
                _scenes.Remove(scene);
            }
        }
    }
}
