using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public partial class ServiceProvider
    {
        #region Base GetService Methods
        /// <summary>
        /// Attempt to get a service by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Object GetService(Type type)
        {
            // Check to see if this service is already active...
            if(_activeServices.TryGetValue(type, out ServiceConfigurationManager manager))
            {
                return manager.GetInstance();
            }
            
            // Check to see if this service has been registered...
            if(_registeredServices.TryGetValue(type, out ServiceConfiguration configuration))
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
        /// Attempt to get a service by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Object GetService(
            Type type,
            Action<Object, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(type, out ServiceConfigurationManager manager))
            {
                return manager.GetInstance(customSetup, customSetupOrder);
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(type, out ServiceConfiguration configuration))
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
            Type type)
                where T : class
                    => this.GetService(type) as T;

        public T GetService<T>(
            Type type,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.GetService(type, (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;

        public T GetService<T>()
                where T : class
                    => this.GetService(typeof(T)) as T;

        public T GetService<T>(
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.GetService(typeof(T), (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;

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
            Type type)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(type));

        public Lazy<T> GetServiceLazy<T>(
            Type type,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(type, customSetup, customSetupOrder));

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
            Type type,
            out T instance)
                where T : class
                    => instance = this.GetService(type) as T;

        public void Service<T>(
            Type type,
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetService(type, customSetup, customSetupOrder) as T;

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
                    => instance = this.GetService(typeof(T)) as T;

        public void Service<T>(
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetService(typeof(T), customSetup, customSetupOrder) as T;
        #endregion

        #region ServiceLazy Methods
        public void ServiceLazy<T>(
            Type type,
            out Lazy<T> instance)
                where T : class
                    => instance = this.GetServiceLazy<T>(type);

        public void ServiceLazy<T>(
            Type type,
            out Lazy<T> instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.GetServiceLazy<T>(type, customSetup, customSetupOrder);

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
