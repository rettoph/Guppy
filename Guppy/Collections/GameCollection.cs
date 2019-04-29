using Guppy.Enums;
using Guppy.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class GameCollection : UniqueObjectCollection<Game>
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
        public TGame Create<TGame>()
            where TGame : Game
        {
            // Create the new game
            var game = _provider.CreateScope().ServiceProvider.GetGame<TGame>();

            if (game == null)
                throw new Exception($"Error creating Game<{typeof(TGame).Name}> ");

            // return the new game
            return game;
        }

        #region Collection Methods
        public override void Add(Game item)
        {
            if (item.InitializationStatus != InitializationStatus.NotReady)
                throw new Exception($"Unable to add Game too GameCollection! Game has been initialized.");

            base.Add(item);

            // When a new game gets added, we must initialize it
            item.TryBoot();
            item.TryPreInitialize();
            item.TryInitialize();
            item.TryPostInitialize();
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
