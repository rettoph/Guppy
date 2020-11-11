using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public partial class ServiceProvider
    {

        #region GetService Methods
        private ServiceConfiguration _configuration;
        private static Action<Object, ServiceProvider, ServiceConfiguration> SetupFrom<T>(Action<T, ServiceProvider, ServiceConfiguration> setup)
            where T : class
                => setup == default ? default : new Action<Object, ServiceProvider, ServiceConfiguration>((o, p, c) => setup(o as T, p, c));

        public Object GetService(
            UInt32 id, 
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
        {
            if (!_services.TryGetValue(id, out _configuration))
                if (!_lookups.TryGetValue(id, out _configuration))
                    return default;

            return _configuration.GetInstance(this, setup, setupOrder);
        }

        public Object GetService(
            String name,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                => this.GetService(ServiceConfiguration.GetId(name), setup, setupOrder);

        public Object GetService(
            Type type,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                => this.GetService(ServiceConfiguration.GetId(type), setup, setupOrder);

        public T GetService<T>(
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                where T : class
                    => this.GetService(ServiceConfiguration.GetId(typeof(T)), SetupFrom(setup), setupOrder) as T;
        public T GetService<T>(
            UInt32 id,
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                where T : class
                    => this.GetService(id, SetupFrom(setup), setupOrder) as T;

        public T GetService<T>(
            String name,
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                where T : class
                    => this.GetService(ServiceConfiguration.GetId(name), SetupFrom(setup), setupOrder) as T;
        #endregion

        #region Service Methods
        public void Service<T>(
            UInt32 id, 
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
            where T : class
                => instance = this.GetService<T>(id, setup, setupOrder);

        public void Service<T>(
            String name,
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
            where T : class
                => instance = this.GetService<T>(name, setup, setupOrder);

        public void Service<T>(
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
            where T : class
                => instance = this.GetService<T>(setup, setupOrder);
        #endregion
    }
}
