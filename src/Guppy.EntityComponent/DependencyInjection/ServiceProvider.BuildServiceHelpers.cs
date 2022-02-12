using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.DependencyInjection
{
    public partial class ServiceProvider
    {
        #region Object Methods
        /// <summary>
        /// Return a new instance reguardless of the lifetime.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Object BuildService(Type type)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(type, out ServiceConfigurationManager manager))
            {
                return manager.BuildInstance();
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(type, out ServiceConfiguration configuration))
            {
                return configuration.BuildInstance(this);
            }

            return default;
        }

        /// <summary>
        /// Attempt to get a service by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object BuildService(UInt32 id)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(id, out ServiceConfigurationManager manager))
            {
                return manager.BuildInstance();
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(id, out ServiceConfiguration configuration))
            {
                return configuration.BuildInstance(this);
            }

            return default;
        }

        /// <summary>
        /// Attempt to get a service by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Object BuildService(
            Type type,
            Action<Object, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(type, out ServiceConfigurationManager manager))
            {
                return manager.BuildInstance(customSetup, customSetupOrder);
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(type, out ServiceConfiguration configuration))
            {
                return configuration.BuildInstance(this, customSetup, customSetupOrder);
            }

            return default;
        }

        /// <summary>
        /// Attempt to get a service by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object BuildService(
            UInt32 id,
            Action<Object, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
        {
            // Check to see if this service is already active...
            if (_activeServices.TryGetValue(id, out ServiceConfigurationManager manager))
            {
                return manager.BuildInstance(customSetup, customSetupOrder);
            }

            // Check to see if this service has been registered...
            if (_registeredServices.TryGetValue(id, out ServiceConfiguration configuration))
            {
                return configuration.BuildInstance(this as ServiceProvider, customSetup, customSetupOrder);
            }

            return default;
        }
        #endregion

        #region GetService Generic Helpers
        public T BuildService<T>(
            Type type)
                where T : class
                    => this.BuildService(type) as T;

        public T BuildService<T>(
            Type type,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.BuildService(type, (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;

        public T BuildService<T>()
                where T : class
                    => this.BuildService(typeof(T)) as T;

        public T BuildService<T>(
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.BuildService(typeof(T), (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;

        public T BuildService<T>(
            UInt32 id)
                where T : class
                    => this.BuildService(id) as T;

        public T BuildService<T>(
            UInt32 id,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => this.BuildService(id, (i, p, c) => customSetup(i as T, p, c), customSetupOrder) as T;
        #endregion

        #region Service Methods
        public void BuildService<T>(
            Type type,
            out T instance)
                where T : class
                    => instance = this.BuildService(type) as T;

        public void BuildService<T>(
            Type type,
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.BuildService(type, customSetup, customSetupOrder) as T;

        public void BuildService<T>(
            UInt32 id,
            out T instance)
                where T : class
                    => instance = this.BuildService(id) as T;

        public void BuildService<T>(
            UInt32 id,
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.BuildService(id, customSetup, customSetupOrder) as T;

        public void BuildService<T>(
            out T instance)
                where T : class
                    => instance = this.BuildService(typeof(T)) as T;

        public void BuildService<T>(
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder = 0)
                where T : class
                    => instance = this.BuildService(typeof(T), customSetup, customSetupOrder) as T;
        #endregion
    }
}
