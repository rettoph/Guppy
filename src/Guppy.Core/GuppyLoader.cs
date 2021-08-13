using Guppy.Attributes;
using Guppy.Extensions.System.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.System;

namespace Guppy
{
    /// <summary>
    /// The main Guppy object manager.
    /// </summary>
    public sealed class GuppyLoader
    {
        #region Private Fields
        private GuppyServiceCollection _services;
        private HashSet<IServiceLoader> _serviceLoaders;
        #endregion

        #region Public Attributes
        public Boolean Initialized { get; private set; }
        public GuppyServiceCollection Services
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

            _services = new GuppyServiceCollection();

            _serviceLoaders = new HashSet<IServiceLoader>();
            AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<IServiceLoader, AutoLoadAttribute>()
                    .Select(t => Activator.CreateInstance(t) as IServiceLoader)
                    .ForEach(sl => this.RegisterServiceLoader(sl));
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Manually add a service loader into Guppy. Note, this only
        /// works pre initialization.
        /// </summary>
        /// <param name="serviceLoader"></param>
        public void RegisterServiceLoader(IServiceLoader serviceLoader)
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to add service loaders after Guppy has been initialized.");

            _serviceLoaders.Add(serviceLoader);
        }

        /// <summary>
        /// One time call to initialize Guppy. This will run all service loaders.
        /// </summary>
        /// <returns></returns>
        public GuppyLoader Initialize()
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to run GuppyLoader.Initialize multiple times.");

            // Iterate through all contained service loaders and configure the services
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.RegisterServices(this.Services);

            this.Initialized = true;

            return this;
        }

        /// <summary>
        /// Build a brand new service provider and automatically
        /// run any service loader configurations to it.
        /// </summary>
        /// <returns></returns>
        public GuppyServiceProvider BuildServiceProvider()
        {
            var provider = _services.BuildServiceProvider();

            // Iterate through all contained service loaders and configure the provider
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureProvider(provider);

            return provider;
        }
        #endregion
    }
}
