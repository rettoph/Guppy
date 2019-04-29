using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class GameFactory
    {
        private IServiceProvider _provider;

        public GameFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TGame Create<TGame>()
            where TGame : Game
        {
            return ActivatorUtilities.CreateInstance(_provider, typeof(TGame)) as TGame;
        }
    }
}
