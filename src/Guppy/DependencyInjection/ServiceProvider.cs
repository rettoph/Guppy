using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceProvider : IServiceProvider
    {
        #region Internal Fields
        internal Dictionary<UInt32, ServiceConfiguration> factories;
        internal Dictionary<Type, Object> scopedInstances;
        internal Dictionary<Type, Object> singletonInstances;
        #endregion

        #region Constructors
        internal ServiceProvider(Dictionary<Type, Object> singletonInstances, Dictionary<UInt32, ServiceConfiguration> factories)
        {
            this.factories = factories;
            this.singletonInstances = singletonInstances;
            this.scopedInstances = new Dictionary<Type, Object>();
        }
        internal ServiceProvider(ServiceCollection collection)
        {
            this.scopedInstances = new Dictionary<Type, Object>();
            this.singletonInstances = new Dictionary<Type, Object>();

            var services = collection.ServiceDescriptors
                .GroupBy(s => s.ServiceType)
                .ToDictionary(
                    keySelector: kvp => kvp.Key,
                    elementSelector: kvp => kvp.OrderBy(s => s.Priority).First());

            var configurationCache = new List<ConfigurationDescriptor>();
            this.factories = collection.ConfigurationDescriptors
                .Where(c => !String.IsNullOrEmpty(c.Name))
                .GroupBy(c => c.Name)
                .Select(g =>
                { // Iterate through every named configuration...
                    configurationCache.Clear();
                    var serviceType = g.First().ServiceType;


                    g.ForEach(c =>
                    { // Iterate through every service descriptor within the current named condition...
                        if (c.ServiceType == serviceType || c.ServiceType.IsAssignableFrom(serviceType))
                        { // If the configuration service type matches the initial configuration service type...
                            configurationCache.Add(c);
                        }
                        else if(serviceType.IsAssignableFrom(c.ServiceType))
                        {
                            serviceType = c.ServiceType;
                            configurationCache.Add(c);
                        }
                        else
                        { // If there is a service type mismatch.
                            throw new ArgumentException($"ConfigurationDescriptor ServiceType mismatch. ServiceDescriptor({c.Name}) failed registration, expecting ServiceType<{serviceType.Name}> but recieved ServiceType<{c.ServiceType.Name}>.");
                        }
                    });

                    // Now add all global configuration methods...
                    configurationCache.AddRange(collection.ConfigurationDescriptors.Where(c => String.IsNullOrEmpty(c.Name) && c.ServiceType.IsAssignableFrom(serviceType)));

                    return new ServiceConfiguration(g.Key, services[serviceType], configurationCache.OrderBy(c => c.Priority).ToArray());
                })
                .ToDictionary(
                    keySelector: c => c.Id,
                    elementSelector: c => c);

            // Clear the configuration cache, just to be safe...
            configurationCache.Clear();

            services.Keys.ForEach(s =>
            { // Ensure that every service at least has a default empty factory instance
                if (!this.factories.ContainsKey(ServiceConfiguration.GetId(s.FullName)))
                { // If there is no factory defined for this service...
                    var configuration = new ServiceConfiguration(s.FullName, services[s], collection.ConfigurationDescriptors.Where(c => String.IsNullOrEmpty(c.Name) && c.ServiceType.IsAssignableFrom(s)).OrderBy(c => c.Priority).ToArray());
                    this.factories.Add(configuration.Id, configuration);
                }
            });
        }
        #endregion

        #region IServiceProvider Implementation
        public Object GetService(Type serviceType)
        {
            return this.GetService<Object>(serviceType.FullName);
        }

        public Object GetService(Type serviceType, Action<Object, ServiceProvider, ServiceConfiguration> setup)
        {
            return this.GetService<Object>(serviceType.FullName, setup);
        }
        #endregion

        #region Helper Methods
        public ServiceProvider CreateScope()
        {
            return new ServiceProvider(this.singletonInstances, this.factories);
        }
        #endregion

        #region GetService Methods
        /// <summary>
        /// Grabs an existing service of Type T, or 
        /// returns a new service registered under
        /// the configuration name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public T GetService<T>(UInt32 configurationId, Action<T, ServiceProvider, ServiceConfiguration> setup = null)
        {
            var factory = this.GetFactory(configurationId); ;
            return (T)factory.ServiceDescriptor.GetInstance(this, factory, (i, p, c) => setup?.Invoke((T)i, p, c));
            // return (T)this.services[configuration.ServiceType].GetInstance(this, factory);
        }

        /// <summary>
        /// Grabs an existing service of Type T, or 
        /// returns a new service registered under
        /// the configuration name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public T GetService<T>(String configuration, Action<T, ServiceProvider, ServiceConfiguration> setup = null)
        {
            return this.GetService<T>(xxHash.CalculateHash(Encoding.UTF8.GetBytes(configuration)), setup);
        }

        /// <summary>
        /// Returns an instance of the requested service type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>(Action<T, ServiceProvider, ServiceConfiguration> setup = null)
        {
            return (T)this.GetService(typeof(T), (i, p, c) => setup?.Invoke((T)i, p, c));
        }
        /// <summary>
        /// Automaitcally set the out value via the intenral GetService method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public void Service<T>(out T service, Action<T, ServiceProvider, ServiceConfiguration> setup = null)
        {
            service = this.GetService<T>(setup);
        }
        #endregion
    }
}
