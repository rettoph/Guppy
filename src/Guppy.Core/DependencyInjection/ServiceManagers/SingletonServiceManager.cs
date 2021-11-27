using Guppy.DependencyInjection.Interfaces;
using Guppy.DependencyInjection.ServiceConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.ServiceManagers
{
    internal class SingletonServiceManager : ScopedServiceManager
    {
        #region Constructors
        internal SingletonServiceManager(
            IServiceConfiguration configuration,
            GuppyServiceProvider provider, 
            Type[] generics) : base(configuration, provider, generics)
        {
            if (!provider.IsRoot)
                throw new ArgumentException($"Unable to create SingletonServiceManager off non Root service provider.");
        }
        #endregion
    }
}
