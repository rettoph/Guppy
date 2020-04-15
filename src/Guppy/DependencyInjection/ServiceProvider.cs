using Guppy.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceProvider : IServiceProvider
    {
        #region Private Fields
        private Dictionary<Type, ServiceDescriptor> _services;
        private Dictionary<Type, Configuration> _defaultConfigurations;
        private Dictionary<UInt32, Configuration> _configurations;
        #endregion

        #region Internal Fields
        internal Dictionary<Type, Object> scopedInstances;
        internal Dictionary<Type, Object> singletonInstances;
        #endregion

        #region Constructors
        internal ServiceProvider(Dictionary<Type, Object> singletonInstances, Dictionary<Type, ServiceDescriptor> services, Dictionary<Type, Configuration> defaultConfigurations, Dictionary<UInt32, Configuration> configurations)
        {
            _services = services;
            _defaultConfigurations = defaultConfigurations;
            _configurations = configurations;
            this.singletonInstances = singletonInstances;
            this.scopedInstances = new Dictionary<Type, Object>();
        }
        internal ServiceProvider(ServiceCollection collection)
        {
            this.scopedInstances = new Dictionary<Type, Object>();
            this.singletonInstances = new Dictionary<Type, Object>();

            _services = collection.ServiceDescriptors
                .GroupBy(s => s.ServiceType)
                .ToDictionary(
                    keySelector: kvp => kvp.Key,
                    elementSelector: kvp => kvp.OrderBy(s => s.Priority).First());

            _defaultConfigurations = collection.ConfigurationDescriptors
                .Where(c => String.IsNullOrEmpty(c.Name))
                .GroupBy(
                    keySelector: c => c.ServiceType)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => new Configuration(String.Empty, g.OrderBy(c => c.Priority).ToArray()));
            _services.Keys.ForEach(s =>
            { // Ensure that every service has a default empty configuration array
                if (!_defaultConfigurations.ContainsKey(s))
                    _defaultConfigurations[s] = new Configuration();
            });

            var configurationCache = new List<ConfigurationDescriptor>();
            _configurations = collection.ConfigurationDescriptors
                .Where(c => !String.IsNullOrEmpty(c.Name))
                .GroupBy(c => c.Name)
                .Select(g =>
                { // Iterate through every named configuration...
                    configurationCache.Clear();
                    var serviceType = g.First().ServiceType;


                    g.ForEach(c =>
                    { // Iterate through every service descriptor within the current named condition...
                        if (c.ServiceType == serviceType)
                        { // If the configuration service type matches the initial configuration service type...
                            configurationCache.Add(c);
                        }
                        else
                        { // If there is a service type mismatch.
                            throw new ArgumentException($"ConfigurationDescriptor ServiceType mismatch. ServiceDescriptor({c.Name}) failed registration, expecting ServiceType<{serviceType.Name}> but recieved ServiceType<{c.ServiceType.Name}>.");
                        }
                    });

                    // Now add all global configuration methods...
                    if (_defaultConfigurations.ContainsKey(serviceType))
                        configurationCache.AddRange(_defaultConfigurations[serviceType].Descriptors);

                    return new Configuration(g.Key, configurationCache.OrderBy(c => c.Priority).ToArray());
                })
                .ToDictionary(
                    keySelector: c => c.Id,
                    elementSelector: c => c);

            // Clear the configuration cache, just to be safe...
            configurationCache.Clear();
        }
        #endregion

        #region IServiceProvider Implementation
        public Object GetService(Type serviceType)
        {
            return _services[serviceType].GetInstance(this, _defaultConfigurations[serviceType]);
        }
        #endregion

        #region Helper Methods
        public ServiceProvider CreateScope()
        {
            return new ServiceProvider(this.singletonInstances, _services, _defaultConfigurations, _configurations);
        }
        #endregion

        #region GetService Methods
        /// <summary>
        /// Grabs an existing service of Type T, or 
        /// returns a new service registered under
        /// the configuration name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public T GetService<T>(UInt32 configurationId)
        {
            var configuration = _configurations[configurationId];
            return (T)_services[configuration.ServiceType].GetInstance(this, configuration);
        }

        /// <summary>
        /// Grabs an existing service of Type T, or 
        /// returns a new service registered under
        /// the configuration name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public T GetService<T>(String configuration)
        {
            return this.GetService<T>(xxHash.CalculateHash(Encoding.UTF8.GetBytes(configuration)));
        }

        /// <summary>
        /// Returns an instance of the requested service type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return (T)this.GetService(typeof(T));
        }
        #endregion
    }
}
