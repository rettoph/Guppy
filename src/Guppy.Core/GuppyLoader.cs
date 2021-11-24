using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Guppy
{
    /// <summary>
    /// The main Guppy object manager.
    /// </summary>
    public sealed class GuppyLoader : IDisposable
    {
        #region Private Fields
        private GuppyServiceCollection _services;
        private HashSet<IServiceLoader> _serviceLoaders;
        #endregion

        #region Public Attributes
        public AssemblyHelper AssemblyHelper { get; private set; }
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
        /// <summary>
        /// Create a new GuppyLoader instance.
        /// </summary>
        /// <param name="entry">This will default to <see cref="Assembly.GetEntryAssembly()"/>.</param>
        /// <param name="withAssembliesReferencing">On startup, all assemblies will be recursively checked. Any that matches or references one of these will be tracked for Guppy creation.</param>
        public GuppyLoader(Assembly entry = null, IEnumerable<Assembly> withAssembliesReferencing = default)
        {
            this.Initialized = false;
            this.AssemblyHelper = new AssemblyHelper(
                entry, 
                (withAssembliesReferencing ?? Enumerable.Empty<Assembly>()).Concat(typeof(GuppyLoader).Assembly).ToArray());

            _services = new GuppyServiceCollection();

            _serviceLoaders = new HashSet<IServiceLoader>();
            this.AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<IServiceLoader, AutoLoadAttribute>()
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
                serviceLoader.RegisterServices(this.AssemblyHelper, this.Services);

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

        public void Dispose()
        {
            this.AssemblyHelper.Dispose();
        }
        #endregion
    }
}
