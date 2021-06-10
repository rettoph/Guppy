using Guppy.DependencyInjection.ServiceConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.ServiceManagers
{
    internal class TransientServiceManager : IServiceManager
    {
        #region Private Fields
        protected readonly IServiceConfiguration configuration;
        protected readonly ServiceProvider provider;
        protected readonly Type[] generics;
        #endregion

        #region Constructors
        internal TransientServiceManager(
            IServiceConfiguration configuration,
            ServiceProvider provider, 
            Type[] generics)
        {
            this.configuration = configuration;
            this.provider = provider;
            this.generics = generics;
        }
        #endregion

        #region IServiceManager Implmentation
        /// <inheritdoc />
        public virtual Object GetInstance()
            => this.configuration.BuildInstance(this.provider, this.generics);

        /// <inheritdoc />
        public virtual Object GetInstance(Action<Object, ServiceProvider, IServiceConfiguration> setup, Int32 setupOrder)
            => this.configuration.BuildInstance(this.provider, this.generics, setup, setupOrder);
        #endregion

        #region IDisposable Implementation
        public virtual void Dispose()
        {
            //
        }
        #endregion
    }
}
