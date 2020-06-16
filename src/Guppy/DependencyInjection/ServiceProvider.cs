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
        internal Dictionary<Type, ServiceTypeDescriptor> serviceTypeDescriptors;
        internal Dictionary<UInt32, ServiceConfiguration> serviceConfigurations;
        internal Dictionary<Type, Object> scopedInstances;
        internal Dictionary<Type, Object> singletonInstances;
        #endregion

        #region Constructors
        internal ServiceProvider(Dictionary<Type, Object> singletonInstances, Dictionary<UInt32, ServiceConfiguration> configurations)
        {
            this.serviceConfigurations = configurations;
            this.singletonInstances = singletonInstances;
            this.scopedInstances = new Dictionary<Type, Object>();
        }
        internal ServiceProvider(ServiceCollection collection)
        {
            this.scopedInstances = new Dictionary<Type, Object>();
            this.singletonInstances = new Dictionary<Type, Object>();
            this.serviceTypeDescriptors = collection.ServiceTypeDescriptors
                .GroupBy(s => s.ServiceType)
                .ToDictionary(
                    keySelector: kvp => kvp.Key,
                    elementSelector: kvp => kvp.OrderByDescending(s => s.Priority).First()); ;

            var configurationCache = new List<ConfigurationDescriptor>();
            this.serviceConfigurations = collection.ConfigurationDescriptors
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

                    return new ServiceConfiguration(g.Key, this.serviceTypeDescriptors[serviceType], configurationCache.OrderBy(c => c.Priority).ToArray());
                })
                .ToDictionary(
                    keySelector: c => c.Id,
                    elementSelector: c => c);

            // Clear the configuration cache, just to be safe...
            configurationCache.Clear();

            this.serviceTypeDescriptors.Keys.ForEach(s =>
            { // Ensure that every service at least has a default empty factory instance
                if (!this.serviceConfigurations.ContainsKey(ServiceConfiguration.GetId(s.FullName)))
                { // If there is no factory defined for this service...
                    var configuration = new ServiceConfiguration(s.FullName, this.serviceTypeDescriptors[s], collection.ConfigurationDescriptors.Where(c => String.IsNullOrEmpty(c.Name) && c.ServiceType.IsAssignableFrom(s)).OrderBy(c => c.Priority).ToArray());
                    this.serviceConfigurations.Add(configuration.Id, configuration);
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
            return new ServiceProvider(this.singletonInstances, this.serviceConfigurations);
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
            var factory = this.GetServiceConfiguration(configurationId);
            return (T)factory.ServiceTypeDescriptor.GetInstance(this, factory, (i, p, c) => setup?.Invoke((T)i, p, c));
            // return (T)this.services[configuration.ServiceType].GetInstance(this, factory);
        }

        /// <summary>
        /// Grabs an existing service of Type T, or 
        /// returns a new service registered under
        /// the configuration name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        /// <returns></returns>
        public T GetService<T>(String handle, Action<T, ServiceProvider, ServiceConfiguration> setup = null)
        {
            return this.GetService<T>(ServiceConfiguration.GetId(handle), setup);
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
