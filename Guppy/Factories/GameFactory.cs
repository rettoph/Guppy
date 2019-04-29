using Guppy.Collections;
using Guppy.Configurations;
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
            var config = provider.GetRequiredService<GameScopeConfiguration>();

            if (config.Game == null && !this.targetType.IsAbstract)
            { // Create a new scene...
                config.Game = ActivatorUtilities.CreateInstance(provider, typeof(TGame)) as TGame;

                // Auto add the new scene to the scene collection
                var games = provider.GetRequiredService<GameCollection>();
                games.Add(config.Game);

                return config.Game as TGame;
            }
            else if (this.targetType == config.Game.GetType() || this.targetType.IsAssignableFrom(config.Game.GetType()))
            { // Return the pre-existing scene of this type...
                return config.Game as TGame;
            }
            else
            { // Throw an error... the scope already has a scene of a different type...
                throw new Exception("Unable to create new Game instance, scope contains another Game.");
            }
        }

        public static GameFactory<T> BuildFactory<T>()
            where T : Game
        {
            return new GameFactory<T>();
        }
    }
}
