using Guppy.Providers;
using Guppy.Common.Extensions;
using Microsoft.Xna.Framework;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common;

namespace Guppy.Game
{
    internal sealed class Game : IGame
    {
        private IGuppyDrawable[] _drawableComponents;
        private IGuppyUpdateable[] _updateableComonents;

        public IGuppyProvider Guppies { get; private set; }

        public Game(IGuppyProvider guppies, IEnumerable<IGlobalComponent> components)
        {
            this.Guppies = guppies;

            _drawableComponents = components.OfType<IGuppyDrawable>().Sequence(DrawSequence.Draw).ToArray();
            _updateableComonents = components.OfType<IGuppyUpdateable>().Sequence(UpdateSequence.Update).ToArray();
        }

        public void Initialize()
        {
            this.Guppies.Initialize();
        }

        public void Draw(GameTime gameTime)
        {
            foreach(IGuppyDrawable component in _drawableComponents)
            {
                component.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IGuppyUpdateable component in _updateableComonents)
            {
                component.Update(gameTime);
            }
        }

        public void Dispose()
        {
            this.Guppies.Dispose();
        }
    }
}
