using System;
using System.Collections.Generic;
using System.Linq;
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
            Int32 setupOrder = 0,
            Type type = null)
        {
            // Implement some backwards compatibility with Microsoft's DI 
            // For more info visit: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L95
            if (!_services.TryGetValue(id, out _configuration))
                if (!_lookups.TryGetValue(id, out _configuration))
                    return type == default ? default : this.TryCreateGeneric(type) ?? this.TryCreateEnumerable(type);

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
                => this.GetService(ServiceConfiguration.GetId(type), setup, setupOrder, type);

        public T GetService<T>(
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                where T : class
                    => this.GetService(ServiceConfiguration.GetId(typeof(T)), SetupFrom(setup), setupOrder, typeof(T)) as T;

        public T GetService<T>(
            UInt32 id,
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                where T : class
                    => this.GetService(id, SetupFrom(setup), setupOrder, typeof(T)) as T;

        public T GetService<T>(
            String name,
            Action<T, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
                where T : class
                    => this.GetService(ServiceConfiguration.GetId(name), SetupFrom(setup), setupOrder, typeof(T)) as T;
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

        #region GetService Fallbacks
        /// <summary>
        /// Loosly based on Microsoft's DI.
        /// See original here: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L119
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        private Object TryCreateGeneric(Type serviceType)
        {
            ServiceConfiguration configuration;
            if (serviceType.IsConstructedGenericType
                && _services.TryGetValue(ServiceConfiguration.GetId(serviceType.GetGenericTypeDefinition()), out configuration))
                    return configuration.BuildInstance(provider: this, generics: serviceType.GetGenericArguments());

            return null;
        }

        /// <summary>
        /// Loosly based on Microsoft's DI.
        /// See original here: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L130
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        private Object TryCreateEnumerable(Type serviceType)
        {
            if (serviceType.IsConstructedGenericType &&
                serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return _services.Values
                        .Where(sd => sd.Factory.Type.IsAssignableFrom(serviceType.GenericTypeArguments[0]))
                        .Select(sd => sd.BuildInstance(this));

            return null;
        }
        #endregion
    }
}
