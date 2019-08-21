using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy
{
    public class GuppyLoader
    {
        #region Private Fields
        private IServiceCollection _services;
        private HashSet<IServiceLoader> _serviceLoaders;
        #endregion

        #region Public Attributes
        public IServiceCollection Services { get { return _services; } }
        public Boolean Initialized { get; private set; }
        #endregion

        #region Constructors
        public GuppyLoader()
        {
            _services = new ServiceCollection();
            _serviceLoaders = new HashSet<IServiceLoader>(
                collection: AssemblyHelper.GetTypesWithAttribute<IServiceLoader, IsServiceLoaderAttribute>()
                    .Select(t => Activator.CreateInstance(t) as IServiceLoader));
        }
        #endregion

        #region Helper Methods
        public GuppyLoader ConfigureLogger(ILogger logger)
        {
            if (this.Initialized)
                throw new Exception($"Unable to configure Logger after GuppyLoader has been initiailzed.");

            _services.AddSingleton<ILogger>(logger);

            return this;
        }

        public void Initialize()
        {
            if(this.Initialized)
                throw new Exception($"Unable to run GuppyLoader.Initialize more than once.");

            // Iterate through all contained service loaders and configure the services
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureServices(_services);

            this.Initialized = true;
        }

        private IServiceProvider BuildServiceProvider()
        {
            var provider = _services.BuildServiceProvider();

            // Iterate through all contained service loaders and configure the provider
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureProvider(provider);

            return provider;
        }

        public TGame BuildGame<TGame>()
            where TGame : Game
        {
            return this.BuildGame(typeof(TGame)) as TGame;
        }
        public Game BuildGame(Type gameType)
        {
            return this.BuildServiceProvider().GetService(gameType) as Game;
        }
        #endregion
    }
}
