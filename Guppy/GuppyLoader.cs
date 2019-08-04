using Guppy.Attributes;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// Main Guppy loader. This class is used to 
    /// initialize all things Guppy related.
    /// </summary>
    public class GuppyLoader
    {
        #region Static Fields
        private static IServiceLoader[] ServiceLoaders;
        #endregion

        #region Private Fields
        private IServiceCollection _services;
        private IServiceProvider _provider;
        #endregion

        #region Public Fields
        public Boolean Initialized { get; private set; }

        public IServiceCollection Services { get { return _services; } }
        #endregion

        #region Constructor
        public GuppyLoader()
        {
            _services = new ServiceCollection();
        }
        static GuppyLoader()
        {
            // Load all service loaders directly from all references assemblies...
            GuppyLoader.ServiceLoaders = AssemblyHelper.Types.AsParallel()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IServiceLoader).IsAssignableFrom(t))
                .OrderBy(t => {
                    var attribute = t.GetCustomAttribute(typeof(IsServiceLoaderAttribute)) as IsServiceLoaderAttribute;
                    return attribute == null ? 100 : attribute.Priority;
                })
                .Select(t => Activator.CreateInstance(t) as IServiceLoader)
                .ToArray();
        }
        #endregion

        #region Methods
        public void Initialize()
        {
            if (this.Initialized)
                throw new Exception($"Unable to run GuppyLoader.Initialize more than once.");

            // First boot all service loaders...
            foreach (IServiceLoader serviceLoader in GuppyLoader.ServiceLoaders)
                serviceLoader.Boot(_services);

            // Create a new service provider...
            _provider = _services.BuildServiceProvider();

            // Finish initializing the service loaders
            foreach (IServiceLoader serviceLoader in GuppyLoader.ServiceLoaders)
                serviceLoader.PreInitialize(_provider);

            foreach (IServiceLoader serviceLoader in GuppyLoader.ServiceLoaders)
                serviceLoader.Initialize(_provider);

            foreach (IServiceLoader serviceLoader in GuppyLoader.ServiceLoaders)
                serviceLoader.PostInitialize(_provider);

            // Mark the guppy loader as initialized.
            this.Initialized = true;
        }
        #endregion

        #region Helper Methods
        public TGame BuildGame<TGame>(Action<TGame> setup = null)
            where TGame : Game
        {
            return _provider.GetGame<TGame>(setup);
        }
        #endregion
    }
}
