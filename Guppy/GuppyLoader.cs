using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy
{
    class GuppyLoader
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
            _serviceLoaders = new HashSet<IServiceLoader>(
                collection: AssemblyHelper.GetTypesWithAttribute<IServiceLoader, ServiceLoaderAttribute>()
                    .Select(t => Activator.CreateInstance(t) as IServiceLoader));
        }
        #endregion

        #region Helper Methods
        public void Initialize()
        {
            if(this.Initialized)
                throw new Exception($"Unable to run GuppyLoader.Initialize more than once.");

            // Iterate through all contained service loaders and configure the services
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureServices(_services);

            this.Initialized = true;
        }
        #endregion
    }
}
