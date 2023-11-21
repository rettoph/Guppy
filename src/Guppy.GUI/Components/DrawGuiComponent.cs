using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Common.Extensions;
using Guppy.GUI.Enums;
using Guppy.MonoGame;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Components
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal sealed class DrawGuiComponent : IGuppyComponent, IDrawableComponent
    {
        private IGuiComponent[] _components;
        private ImGuiBatch _batch;

        public DrawGuiComponent(ImGuiBatch batch)
        {
            _components = Array.Empty<IGuiComponent>(); ;
            _batch = batch;
        }

        public void Initialize(IGuppy guppy)
        {
            _components = guppy.Components.OfType<IGuiComponent>().Sequence(GuiSequence.OnGui).ToArray();
        }

        public void Draw(GameTime gameTime)
        {
            _batch.Begin(gameTime);

            foreach(IGuiComponent component in _components)
            {
                component.DrawGui();
            }

            _batch.End();
        }
    }
}
