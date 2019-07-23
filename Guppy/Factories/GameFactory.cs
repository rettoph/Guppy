using Guppy.Collections;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class GameFactory : InitializableFactory<Game>
    {
        public override TGame Create<TGame>(IServiceProvider provider, params Object[] args)
        {
            var config = provider.GetRequiredService<GameScopeConfiguration>();

            if (config.Game == null && !this.targetType.IsAbstract)
            { // Create a new game...
                config.Game = base.Create<TGame>(provider, args);

                return config.Game as TGame;
            }
            else if (this.targetType == config.Game.GetType() || this.targetType.IsAssignableFrom(config.Game.GetType()))
            { // Return the pre-existing game of this type...
                return config.Game as TGame;
            }
            else
            { // Throw an error... the scope already has a game of a different type...
                throw new Exception("Unable to create new Game instance, scope contains another Game.");
            }
        }
    }
}
