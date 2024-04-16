using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Providers;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Components.Game
{
    [AutoLoad]
    internal class StepGameGuppiesComponent : GlobalComponent, IGuppyDrawable, IGuppyUpdateable
    {
        private readonly IGuppyProvider _guppies;
        private readonly List<IGameGuppy> _frameables;

        public StepGameGuppiesComponent(IGuppyProvider guppies)
        {
            _guppies = guppies;
            _frameables = _guppies.OfType<IGameGuppy>().ToList();

            _guppies.OnGuppyCreated += HandleGuppyCreated;
            _guppies.OnGuppyDestroyed += HandleGuppyDestroyed;
        }

        public void Initialize()
        {
        }

        public void Draw(GameTime gameTime)
        {
            foreach (IGameGuppy frameable in _frameables)
            {
                frameable.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IGameGuppy frameable in _frameables)
            {
                frameable.Update(gameTime);
            }
        }

        private void HandleGuppyCreated(IGuppyProvider sender, IGuppy args)
        {
            if (args is IGameGuppy frameable)
            {
                _frameables.Add(frameable);
            }
        }

        private void HandleGuppyDestroyed(IGuppyProvider sender, IGuppy args)
        {
            if (args is IGameGuppy frameable)
            {
                _frameables.Remove(frameable);
            }
        }
    }
}
