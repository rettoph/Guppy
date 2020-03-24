using Guppy.Attributes;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions;

namespace Guppy
{
    /// <summary>
    /// The main Guppy object manager.
    /// </summary>
    public sealed class GuppyLoader
    {
        #region Private Fields
        private ServiceCollection _services;
        private ServiceProvider _provider;
        private HashSet<IServiceLoader> _serviceLoaders;
        #endregion

        #region Public Attributes
        public Boolean Initialized { get; private set; }
        public ServiceCollection Services
        {
            get
            {
                if (this.Initialized)
                    throw new InvalidOperationException("Unable to access ServiceCollection after Guppy has been initialized.");

                return _services;
            }
        }
        #endregion

        #region Constructor
        public GuppyLoader()
        {
            this.Initialized = false;

            _services = new ServiceCollection();

            _serviceLoaders = new HashSet<IServiceLoader>();
            AssemblyHelper.GetTypesWithAutoLoadAttribute<IServiceLoader, AutoLoadAttribute>()
                    .Select(t => Activator.CreateInstance(t) as IServiceLoader)
                    .ForEach(sl => this.AddServiceLoader(sl));
        }
        #endregion

        #region Helper Methods
        public T BuildGame<T>(Action<ServiceProvider, T> setup = null)
            where T : Game
        {
            return _provider.CreateScope().GetService<T>(setup);
        }

        /// <summary>
        /// Manually add a service loader into Guppy. Note, this only
        /// works pre initialization.
        /// </summary>
        /// <param name="serviceLoader"></param>
        public void AddServiceLoader(IServiceLoader serviceLoader)
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to add service loaders after Guppy has been initialized.");

            _serviceLoaders.Add(serviceLoader);
        }

        public GuppyLoader Initialize()
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to run GuppyLoader.Initialize multiple times.");

            // Iterate through all contained service loaders and configure the services
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureServices(this.Services);

            // Create a new service provider
            _provider = new ServiceProvider(_services);

            // Iterate through all contained service loaders and configure the provider
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureProvider(_provider);

            this.Initialized = true;

            return this;
        }
        #endregion
    }
}
