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
        private GlobalOptions _options;
        #endregion

        #region Constructor
        public GameFactory(GlobalOptions options, IPoolManager<Game> pools, IServiceProvider provider) : base(pools, provider)
        {
            _options = options;
            _provider = provider;
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            // If the global options doesnt already have a game defined... build a new one
            if (_options.Game == null)
                base.Build<T>(
                    provider: provider, 
                    pool: pool, 
                    setup: g =>
                    {
                        _options.Game = g;

                        setup?.Invoke(g);
                    },
                    create: create);

            // Return the saved global game instance
            return _options.Game as T;
        }
    }
}
