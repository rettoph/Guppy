using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Common.Extensions;
using Guppy.GUI;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Guppy.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components.Game
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal class DrawGuiComponent : GlobalComponent, IDrawableComponent
    {
        private readonly ImGuiBatch _batch;
        private readonly IGuppyProvider _guppies;
        private readonly List<MonoGameGuppy> _frameables;
        private IGuiComponent[] _components;

        public DrawGuiComponent(IGuppyProvider guppies, ImGuiBatch batch)
        {
            _batch = batch;
            _guppies = guppies;
            _frameables = _guppies.OfType<MonoGameGuppy>().ToList();
            _components = Array.Empty<IGuiComponent>();

            _guppies.OnGuppyCreated += HandleGuppyCreated;
            _guppies.OnGuppyDestroyed += HandleGuppyDestroyed;
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components.OfType<IGuiComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void Draw(GameTime gameTime)
        {
            _batch.Begin(gameTime);
            foreach(IGuiComponent component in _components)
            {
                component.DrawGui(gameTime);
            }

            foreach (MonoGameGuppy frameable in _frameables)
            {
                frameable.DrawGui(gameTime);
            }
            _batch.End();
        }

        private void HandleGuppyCreated(IGuppyProvider sender, IGuppy args)
        {
            if (args is MonoGameGuppy frameable)
            {
                _frameables.Add(frameable);
            }
        }

        private void HandleGuppyDestroyed(IGuppyProvider sender, IGuppy args)
        {
            if (args is MonoGameGuppy frameable)
            {
                _frameables.Remove(frameable);
            }
        }
    }
}
