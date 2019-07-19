using Guppy.Enums;
using Guppy.Extensions;
using Guppy.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class GameCollection : InitializableCollection<Game>
    {
        private IServiceProvider _provider;

        public GameCollection(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Create a new instance of a game, complete with
        /// an internal scope.
        /// </summary>
        /// <typeparam name="TGame"></typeparam>
        /// <returns></returns>
        public TGame Create<TGame>(params Object[] args)
            where TGame : Game
        {
            // Create the new game
            var game = _provider.GetRequiredService<GameFactory<TGame>>().CreateCustom(_provider.CreateScope().ServiceProvider, args);

            if (game == null)
                throw new Exception($"Error creating Game<{typeof(TGame).Name}> ");

            // auto add the game to the collection
            this.Add(game);

            // return the new game
            return game;
        }

        #region Collection Methods
        public override void Add(Game item)
        {
            base.Add(item);
        }

        public override bool Remove(Game item)
        {
            if (base.Remove(item))
            { // When a game gets removed, we must dispose of it...
                item.Dispose();

                return true;
            }

            return false;
        }
        #endregion
    }
}
