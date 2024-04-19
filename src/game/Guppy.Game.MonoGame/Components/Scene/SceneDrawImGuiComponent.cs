using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal class SceneDrawImGuiComponent : SceneComponent, IGuppyDrawable
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

            _components = _scene.Components.OfType<IImGuiComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void Draw(GameTime gameTime)
        {
            foreach(IImGuiComponent component in _components)
            {
                component.DrawImGui(gameTime);
            }
        }
    }
}
