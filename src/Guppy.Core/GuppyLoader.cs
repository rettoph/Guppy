using Guppy.Attributes;
using Guppy.EntityComponent.Attributes;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.EntityComponent.Interfaces;
using Guppy.Interfaces;
using Guppy.ServiceLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public class GuppyLoader : IDisposable
    {
        #region Private Fields
        private ServiceProviderBuilder _services;
        private HashSet<IGuppyLoader> _loaders;
        private HashSet<IGuppyInitializer> _initializers;
        #endregion

        #region Public Attributes
        public AssemblyHelper AssemblyHelper { get; private set; }
        public Boolean Initialized { get; private set; }
        public ServiceProviderBuilder Services
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

            _services = new ServiceProviderBuilder();

            _loaders = new HashSet<IGuppyLoader>();
            this.AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<IGuppyLoader, AutoLoadAttribute>()
                    .Select(t => Activator.CreateInstance(t) as IGuppyLoader)
                    .ForEach(sl => this.RegisterServiceLoader(sl));

            _initializers = new HashSet<IGuppyInitializer>();
            this.AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<IGuppyInitializer, AutoLoadAttribute>()
                .Select(t => Activator.CreateInstance(t) as IGuppyInitializer)
                .ForEach(sl => this.RegisterInitializer(sl));
        }
        #endregion

        #region Helper Methods
        public void RegisterInitializer(IGuppyInitializer initializer)
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to add initizliaers after Guppy has been initialized.");

            _initializers.Add(initializer);
        }

        /// <summary>
        /// Manually add a service loader into Guppy. Note, this only
        /// works pre initialization.
        /// </summary>
        /// <param name="serviceLoader"></param>
        public void RegisterServiceLoader(IGuppyLoader serviceLoader)
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to add service loaders after Guppy has been initialized.");

            _loaders.Add(serviceLoader);
        }

        /// <summary>
        /// One time call to initialize Guppy. This will run all service loaders.
        /// </summary>
        /// <returns></returns>
        public GuppyLoader Initialize()
        {
            if (this.Initialized)
                throw new InvalidOperationException("Unable to run GuppyLoader.Initialize multiple times.");

            // Iterate through all GuppyInitializers.
            // Note, this is where IServiceLoader.RegisterServices gets called now.
            // See ServiceLoaderGuppyInitializer.cs
            foreach (IGuppyInitializer initializer in _initializers)
            {
                initializer.PreInitialize(this.AssemblyHelper, this.Services, _loaders);
            }

            this.Initialized = true;

            return this;
        }

        /// <summary>
        /// Build a brand new service provider and automatically
        /// run any service loader configurations to it.
        /// </summary>
        /// <returns></returns>
        public ServiceProvider BuildServiceProvider()
        {
            // Now we can attempt to build the provider
            ServiceProvider provider = _services.Build();

            // Iterate through all GuppyInitializers.
            foreach (IGuppyInitializer initializer in _initializers)
            {
                initializer.PostInitialize(provider, _loaders);
            }

            return provider;
        }

        public void Dispose()
        {
            this.AssemblyHelper.Dispose();
        }
        #endregion
    }
}
