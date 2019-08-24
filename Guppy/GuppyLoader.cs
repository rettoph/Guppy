using Guppy.Attributes;
using Guppy.Extensions.Collection;
using Guppy.Factories;
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
        public Boolean Initialized { get; private set; }
        #endregion

        #region Constructors
        public GuppyLoader()
        {
            _services = new ServiceCollection();
            _services.AddSingleton(this);
            _serviceLoaders = new HashSet<IServiceLoader>();
            AssemblyHelper.GetTypesWithAttribute<IServiceLoader, IsServiceLoaderAttribute>()
                    .Select(t => Activator.CreateInstance(t) as IServiceLoader)
                    .ForEach(sl => this.AddServiceLoader(sl));
        }
        #endregion

        #region Helper Methods
        public void AddServiceLoader(IServiceLoader serviceLoader)
        {
            if (this.Initialized)
                throw new Exception("Unable to add service loaders after Guppy has been initialized.");

            _serviceLoaders.Add(serviceLoader);
        }

        public GuppyLoader ConfigureLogger<TLogger>()
            where TLogger : class, ILogger
        {
            if (this.Initialized)
                throw new Exception($"Unable to configure Logger after GuppyLoader has been initiailzed.");

            _services.AddSingleton<ILogger, TLogger>();

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
            if (!this.Initialized)
                throw new Exception("Unable to build Guppy ServiceProvider before GuppyLoader.Initialize has been called.");

            var provider = _services.BuildServiceProvider();

            // Iterate through all contained service loaders and configure the provider
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureProvider(provider);

            return provider;
        }

        public Game BuildGame(Type gameType, Action<Game> setup = null)
        {
            return this.BuildServiceProvider().GetService<GameFactory>().Build(gameType, setup);
        }

        public TGame BuildGame<TGame>(Action<TGame> setup = null)
            where TGame : Game
        {
            return this.BuildServiceProvider().GetService<GameFactory>().Build<TGame>(setup);
        }
        #endregion
    }
}
