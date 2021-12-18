using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal class SingletonServiceConfigurationManager : ScopedServiceConfigurationManager
    {
        #region Constructors
        internal SingletonServiceConfigurationManager(
            ServiceConfiguration configuration,
            ServiceProvider provider) : base(configuration, provider)
        {
            if (!provider.IsRoot)
                throw new ArgumentException($"Unable to create SingletonServiceManager off non Root service provider.");
        }
        #endregion
    }
}
