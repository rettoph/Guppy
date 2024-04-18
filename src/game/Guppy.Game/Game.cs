using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Providers;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game
{
    internal sealed class Game : IGame
    {
        private bool _disposing;

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
            if (_disposing)
            {
                return;
            }

            _disposing = true;
            this.Guppies.Dispose();
        }
    }
}
