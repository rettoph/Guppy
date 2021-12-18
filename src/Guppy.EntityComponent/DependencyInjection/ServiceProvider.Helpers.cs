using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public partial class ServiceProvider
    {
        #region Object Methods
        /// <summary>
        /// Attempt to get a service by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Object GetService(String name)
        {
            // Check to see if this service is already active...
            if(_activeServices.TryGetValue(name, out ServiceConfigurationManager manager))
            {
                return manager.GetInstance();
            }
            
            // Check to see if this service has been registered...
            if(_registeredServices.TryGetValue(name, out ServiceConfiguration configuration))
            {
                return configuration.GetInstance(this);
            }

            return default;
        }

        /// <summary>
        /// Attempt to get a service by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetService(UInt32 id)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(id, out ServiceConfigurationManager manager))
            {
                return manager.GetInstance();
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(id, out ServiceConfiguration configuration))
            {
                return configuration.GetInstance(this);
            }

            return default;
        }

        /// <summary>
        /// Attempt to get a service by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Object GetService(
            String name,
            Action<Object, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(name, out ServiceConfigurationManager manager))
            {
                return manager.GetInstance(customSetup, customSetupOrder);
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(name, out ServiceConfiguration configuration))
            {
                return configuration.GetInstance(this, customSetup, customSetupOrder);
            }

            return default;
        }

        /// <summary>
        /// Attempt to get a service by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetService(
            UInt32 id,
            Action<Object, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(id, out ServiceConfigurationManager manager))
            {
                return manager.GetInstance(customSetup, customSetupOrder);
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(id, out ServiceConfiguration configuration))
            {
                return configuration.GetInstance(this as ServiceProvider, customSetup, customSetupOrder);
            }

            return default;
        }
        #endregion

        #region GetService Generic Helpers
        public T GetService<T>(
            String name)
                where T : class
                    => this.GetService(name) as T;

        public T GetService<T>(
            String name,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.GetService(name, (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;

        public T GetService<T>()
                where T : class
                    => this.GetService(typeof(T).FullName) as T;

        public T GetService<T>(
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.GetService(typeof(T).FullName, (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;

        public T GetService<T>(
            UInt32 id)
                where T : class
            => this.GetService(id) as T;

        public T GetService<T>(
            UInt32 id,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.GetService(id, (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;
        #endregion

        #region GetService Generic Helpers
        public Lazy<T> GetServiceLazy<T>(
            String name)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(name));

        public Lazy<T> GetServiceLazy<T>(
            String name,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(name, customSetup, customSetupOrder));

        public Lazy<T> GetServiceLazy<T>()
                where T : class
                    => new Lazy<T>(this.GetService<T>);

        public Lazy<T> GetServiceLazy<T>(
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => new Lazy<T>(this.GetService<T>(customSetup, customSetupOrder));

        public Lazy<T> GetServiceLazy<T>(
            UInt32 id)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(id));

        public Lazy<T> GetServiceLazy<T>(
            UInt32 id,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(id, customSetup, customSetupOrder));
        #endregion

        #region Service Methods
        public void Service<T>(
            String name,
            out T instance)
                where T : class
                    => instance = this.GetService(name) as T;

        public void Service<T>(
            String name,
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetService(name, customSetup, customSetupOrder) as T;

        public void Service<T>(
            UInt32 id,
            out T instance)
                where T : class
                    => instance = this.GetService(id) as T;

        public void Service<T>(
            UInt32 id,
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetService(id, customSetup, customSetupOrder) as T;

        public void Service<T>(
            out T instance)
                where T : class
                    => instance = this.GetService(typeof(T).FullName) as T;

        public void Service<T>(
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetService(typeof(T).FullName, customSetup, customSetupOrder) as T;
        #endregion

        #region ServiceLazy Methods
        public void ServiceLazy<T>(
            String name,
            out Lazy<T> instance)
                where T : class
                    => instance = this.GetServiceLazy<T>(name);

        public void ServiceLazy<T>(
            String name,
            out Lazy<T> instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetServiceLazy<T>(name, customSetup, customSetupOrder);

        public void ServiceLazy<T>(
            UInt32 id,
            out Lazy<T> instance)
                where T : class
                    => instance = this.GetServiceLazy<T>(id);

        public void ServiceLazy<T>(
            UInt32 id,
            out Lazy<T> instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetServiceLazy<T>(id, customSetup, customSetupOrder);

        public void ServiceLazy<T>(
            out Lazy<T> instance)
                where T : class
                    => instance = this.GetServiceLazy<T>();

        public void ServiceLazy<T>(
            out Lazy<T> instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetServiceLazy<T>(customSetup, customSetupOrder);
        #endregion
    }
}
