using Guppy.Utilities.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Guppy.Pooling.Interfaces;

namespace Guppy.Factories
{
    internal sealed class GameFactory : DrivenFactory<Game>
    {
        #region Private Fields
        private IServiceProvider _provider;
        #endregion

        #region Constructor
        public GameFactory(IPoolManager<Game> pools, IServiceProvider provider) : base(pools, provider)
        {
            _provider = provider;
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null)
        {
            var options = _provider.GetRequiredService<GlobalOptions>();

            // If the global options doesnt already have a game defined... build a new one
            if (options.Game == null)
                options.Game = base.Build<T>(provider, pool, setup);

            // Return the saved global game instance
            return options.Game as T;
        }
    }
}
