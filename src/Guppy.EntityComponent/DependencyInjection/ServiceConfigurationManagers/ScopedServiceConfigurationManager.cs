using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal class ScopedServiceConfigurationManager : TransientServiceConfigurationManager
    {
        #region Private Fields
        private Object _instance;
        public Func<Object> _getInstance;
        public Func<Action<Object, ServiceProvider, ServiceConfiguration>, Int32, Object> _getInstanceWithCustomSetup;
        #endregion

        #region Constructors
        internal ScopedServiceConfigurationManager(
            ServiceConfiguration configuration,
            ServiceProvider provider) : base(configuration, provider)
        {
            _getInstance = this.BuildInstance;
            _getInstanceWithCustomSetup = this.BuildInstance;
        }
        #endregion

        #region IServiceManager Implmentation
        public override Object GetInstance()
            => _getInstance();

        public override object GetInstance(Action<Object, ServiceProvider, ServiceConfiguration> customSetup, Int32 customSetupOrder)
            => _getInstanceWithCustomSetup(customSetup, customSetupOrder);

        private Object BuildInstance()
        {
            _getInstance = this.ReturnInstance;
            _getInstanceWithCustomSetup = this.ReturnInstance;
            this.configuration.TypeFactory.BuildInstance(this.provider, this.configuration, out _instance);

            return this.ReturnInstance();
        }

        private Object BuildInstance(Action<Object, ServiceProvider, ServiceConfiguration> customSetup, Int32 customSetupOrder)
        {
            _getInstance = this.ReturnInstance;
            _getInstanceWithCustomSetup = this.ReturnInstance;
            this.configuration.TypeFactory.BuildInstance(this.provider, this.configuration, customSetup, customSetupOrder, out _instance);

            return this.ReturnInstance();
        }


        private Object ReturnInstance()
            => _instance;

        private Object ReturnInstance(Action<Object, ServiceProvider, ServiceConfiguration> setup, Int32 setupOrder)
            => _instance;
        #endregion

        #region IDisposable Implementation
        public override void Dispose()
        {
            _instance = null;
            _getInstance = null;
            _getInstanceWithCustomSetup = null;
        }
        #endregion
    }
}
