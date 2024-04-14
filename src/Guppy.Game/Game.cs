using Guppy.Engine.Common;
using Guppy.Engine.Common.Extensions;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Engine.Providers;
using Microsoft.Xna.Framework;
using Guppy.Engine;

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
            try
            {
                this.Guppies.Initialize();
            }
            catch (Exception ex)
            {
                throw GuppyLogger.LogException(ex.Message, ex);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (IGuppyDrawable component in _drawableComponents)
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
