using Guppy.Collections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class GameFactory<TGame> : Factory<TGame>
        where TGame : Game
    {
        private GameFactory()
        {

        }

        public override TGame Create(IServiceProvider provider)
        {
            var game = ActivatorUtilities.CreateInstance(provider, typeof(TGame)) as TGame;

            // Auto add the new scene to the scene collection
            var games = provider.GetRequiredService<GameCollection>();
            games.Add(game);

            return game;
        }

        public static GameFactory<T> BuildFactory<T>()
            where T : Game
        {
            return new GameFactory<T>();
        }
    }
}
