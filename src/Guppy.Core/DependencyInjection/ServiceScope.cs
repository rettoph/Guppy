using Guppy.Extensions.System;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceScope : IServiceScope
    {
        #region Public Properties
        public ServiceProvider ServiceProvider { get; private set; }
        #endregion

        #region Constructors
        public ServiceScope(ServiceProvider parent)
        {
            var key = ServiceConfigurationKey.From<ServiceScope>();

            // Create a new service provider
            this.ServiceProvider = new ServiceProvider(parent);
            this.ServiceProvider.CacheScopedInstance(this.ServiceProvider.ServiceConfigurations[key], this, key.Yield());
        }
        #endregion

        #region IServiceScope Implementation
        IServiceProvider IServiceScope.ServiceProvider => this.ServiceProvider;

        public void Dispose()
        {
            this.ServiceProvider.Dispose();
        }
        #endregion
    }
}
