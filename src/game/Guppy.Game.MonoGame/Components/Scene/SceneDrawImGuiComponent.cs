using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal class SceneDrawImGuiComponent : SceneComponent, IDrawableComponent
    {
        private readonly IScene _scene;
        private IImGuiComponent[] _components;

        public SceneDrawImGuiComponent(IScene scene)
        {
            _scene = scene;
            _components = Array.Empty<IImGuiComponent>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _components = _scene.Components.Sequence<IImGuiComponent, DrawSequence>().ToArray();
        }

        [Sequence<DrawComponentSequence>(DrawComponentSequence.PostDraw)]
        public void Draw(GameTime gameTime)
        {
            foreach (IImGuiComponent component in _components)
            {
                component.DrawImGui(gameTime);
            }
        }
    }
}
