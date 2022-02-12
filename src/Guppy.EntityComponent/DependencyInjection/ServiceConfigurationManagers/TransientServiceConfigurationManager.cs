using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal class TransientServiceConfigurationManager : ServiceConfigurationManager
    {
        #region Private Fields
        protected readonly ServiceConfiguration configuration;
        protected readonly ServiceProvider provider;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public ServiceConfiguration Configuration => this.configuration;
        #endregion

        #region Constructors
        internal TransientServiceConfigurationManager(
            ServiceConfiguration configuration,
            ServiceProvider provider)
        {
            this.configuration = configuration;
            this.provider = provider;
        }
        #endregion

        #region IServiceManager Implmentation
        /// <inheritdoc />
        public override Object GetInstance()
        {
            return this.BuildInstance();
        }

        /// <inheritdoc />
        public override Object GetInstance(Action<Object, ServiceProvider, ServiceConfiguration> customSetup, Int32 customSetupOrder)
        {
            return this.BuildInstance(customSetup, customSetupOrder);
        }

        public override object BuildInstance()
        {
            this.configuration.TypeFactory.BuildInstance(this.provider, this.configuration, out Object instance);
            return instance;
        }

        public override object BuildInstance(Action<Object, ServiceProvider, ServiceConfiguration> customSetup, int customSetupOrder)
        {
            this.configuration.TypeFactory.BuildInstance(this.provider, this.configuration, customSetup, customSetupOrder, out Object instance);
            return instance;
        }
        #endregion

        #region IDisposable Implementation
        public override void Dispose()
        {
            //
        }
        #endregion
    }
}
