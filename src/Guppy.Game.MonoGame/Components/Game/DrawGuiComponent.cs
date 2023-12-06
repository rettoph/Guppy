using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Common.Extensions;
using Guppy.Game.ImGui;
using Guppy.Game;
using Guppy.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common;

namespace Guppy.Game.MonoGame.Components.Game
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal class DrawGuiComponent : GlobalComponent, IGuppyDrawable
    {
        private readonly IImguiBatch _batch;
        private readonly IGuppyProvider _guppies;
        private readonly List<GameGuppy> _frameables;
        private IImGuiComponent[] _components;

        public DrawGuiComponent(IGuppyProvider guppies, IImguiBatch batch)
        {
            _batch = batch;
            _guppies = guppies;
            _frameables = _guppies.OfType<GameGuppy>().ToList();
            _components = Array.Empty<IImGuiComponent>();

            _guppies.OnGuppyCreated += HandleGuppyCreated;
            _guppies.OnGuppyDestroyed += HandleGuppyDestroyed;
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components.OfType<IImGuiComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void Draw(GameTime gameTime)
        {
            _batch.Begin(gameTime);
            foreach(IImGuiComponent component in _components)
            {
                component.DrawImGui(gameTime);
            }

            foreach (GameGuppy frameable in _frameables)
            {
                frameable.DrawGui(gameTime);
            }
            _batch.End();
        }

        private void HandleGuppyCreated(IGuppyProvider sender, IGuppy args)
        {
            if (args is GameGuppy frameable)
            {
                _frameables.Add(frameable);
            }
        }

        private void HandleGuppyDestroyed(IGuppyProvider sender, IGuppy args)
        {
            if (args is GameGuppy frameable)
            {
                _frameables.Remove(frameable);
            }
        }
    }
}
