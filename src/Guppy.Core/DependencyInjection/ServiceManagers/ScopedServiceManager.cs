using Guppy.DependencyInjection.ServiceConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.ServiceManagers
{
    internal class ScopedServiceManager : TransientServiceManager
    {
        #region Private Fields
        private Object _instance;
        public Func<Object> _getInstance;
        public Func<Action<Object, GuppyServiceProvider, IServiceConfiguration>, Int32, Object> _getInstanceWithSetup;
        #endregion

        #region Constructors
        internal ScopedServiceManager(
            IServiceConfiguration configuration,
            GuppyServiceProvider provider,
            Type[] generics) : base(configuration, provider, generics)
        {
            _getInstance = this.BuildInstance;
            _getInstanceWithSetup = this.BuildInstance;
        }
        #endregion

        #region IServiceManager Implmentation
        public override Object GetInstance()
            => _getInstance();

        public override object GetInstance(Action<Object, GuppyServiceProvider, IServiceConfiguration> setup, Int32 setupOrder)
            => _getInstanceWithSetup(setup, setupOrder);

        private Object BuildInstance()
        {
            _getInstance = this.ReturnInstance;
            _getInstanceWithSetup = this.ReturnInstance;
            this.configuration.BuildInstance(this.provider, this.generics, out _instance);

            return this.ReturnInstance();
        }

        private Object BuildInstance(Action<Object, GuppyServiceProvider, IServiceConfiguration> setup, Int32 setupOrder)
        {
            _getInstance = this.ReturnInstance;
            _getInstanceWithSetup = this.ReturnInstance;
            this.configuration.BuildInstance(this.provider, this.generics, setup, setupOrder, out _instance);

            return this.ReturnInstance();
        }


        private Object ReturnInstance()
            => _instance;

        private Object ReturnInstance(Action<Object, GuppyServiceProvider, IServiceConfiguration> setup, Int32 setupOrder)
            => _instance;
        #endregion

        #region IDisposable Implementation
        public override void Dispose()
        {
            _instance = null;
            _getInstance = null;
            _getInstanceWithSetup = null;
        }
        #endregion
    }
}
