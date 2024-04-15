using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Providers;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui;
using Microsoft.Xna.Framework;

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
            foreach (IImGuiComponent component in _components)
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
