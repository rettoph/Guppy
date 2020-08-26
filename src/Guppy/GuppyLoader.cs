﻿using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy
{
    /// <summary>
    /// The main Guppy object manager.
    /// </summary>
    public sealed class GuppyLoader
    {
        #region Private Fields
        private ServiceCollection _services;
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

        public GuppyLoader Initialize()
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to run GuppyLoader.Initialize multiple times.");

            // Iterate through all contained service loaders and configure the services
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureServices(this.Services);

            this.Initialized = true;

            return this;
        }

        public T BuildGame<T>()
            where T : Game
        {
            if (!this.Initialized)
                throw new Exception("Please initialize Guppy before building a game instance.");

            var provider = _services.BuildServiceProvider();

            // Iterate through all contained service loaders and configure the provider
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureProvider(provider);

            return provider.GetService<T>();
        }

        public T BuildGame<T>(String configuration)
            where T : Game
        {
            if (!this.Initialized)
                throw new Exception("Please initialize Guppy before building a game instance.");

            var provider = _services.BuildServiceProvider();

            // Iterate through all contained service loaders and configure the provider
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureProvider(provider);

            return provider.GetService<T>(configuration);
        }
        #endregion
    }
}
