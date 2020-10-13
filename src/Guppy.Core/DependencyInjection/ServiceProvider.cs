using Guppy.Extensions.DependencyInjection;
using Guppy.Exceptions;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.Collections;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceProvider : IServiceProvider
    {
        #region Private Fields
        /// <summary>
        /// A lookup table of all configured factories by type.
        /// </summary>
        private Dictionary<Type, ServiceFactory> _factories;

        private ServiceDescriptor _service;

        private ServiceProvider _root;
        #endregion

        #region Internal Fields
        internal Dictionary<Type, Object> scopedInstances;
        internal Dictionary<Type, Object> singletonInstances;

        /// <summary>
        /// A lookup table of all service descriptors by id.
        /// </summary>
        internal Dictionary<UInt32, ServiceDescriptor> services;

        internal ServiceProvider root => _root ?? this;
        #endregion

        #region Public Fields
        public readonly Guid Id;
        #endregion

        #region Constructors
        internal ServiceProvider(ServiceCollection collection) : base()
        {
            // Create a lookup table of service factories.
            _factories = collection.factories
                .GroupBy(fd => fd.Type)
                .Select(g => g.OrderBy(fd => fd.Priority).First())
                .Select(fd => new ServiceFactory(fd, collection.builders.Where(b => b.Factory.IsAssignableFrom(fd.Type)).ToArray()))
                .ToDictionary(f => f.Type);

            // Create a lookup table of service descriptors
            this.services = collection.services
                .GroupBy(sdd => sdd.Name)
                .Select(g => g.OrderBy(sdd => sdd.Priority).First())
                .Select(sdd => new ServiceDescriptor(
                    data: sdd,
                    provider: this,
                    configurations: collection.configurations.Where(sc => sc.Key.Includes(sdd.Factory, sdd.Name)).OrderBy(sc => sc.Order).ToArray()))
                .ToDictionary(s => s.Id);

            // Finalize provider setup
            this.scopedInstances = new Dictionary<Type, Object>();
            this.singletonInstances = new Dictionary<Type, Object>();

            this.Id = Guid.NewGuid();

            this.AutoBuildServices(ServiceLifetime.Singleton);
        }
        private ServiceProvider(ServiceProvider root)
        {
            _root = root;

            this.scopedInstances = new Dictionary<Type, Object>();

            this.Id = Guid.NewGuid();

            this.AutoBuildServices(ServiceLifetime.Scoped);
        }
        #endregion

        #region GetService Methods
        /// <summary>
        /// Return a new instance of a service 
        /// based on the recieved service id value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public Object GetService(UInt32 id, Action<Object, ServiceProvider, ServiceDescriptor> setup = null)
        {
            this.root.services.TryGetValue(id, out _service);

            return _service?.GetInstance(this, setup);
        }

        /// <summary>
        /// Return a new instance of a service
        /// based on the recieved service name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public Object GetService(String name, Action<Object, ServiceProvider, ServiceDescriptor> setup = null)
            => this.GetService(ServiceDescriptor.GetId(name), setup);

        /// <summary>
        /// Return a new nameless service instance
        /// based on the recieved type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public Object GetService(Type type, Action<Object, ServiceProvider, ServiceDescriptor> setup)
            => this.GetService(ServiceDescriptor.GetId(type), setup);

        /// <summary>
        /// Return a new nameless service instance
        /// based on the recieved type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Object GetService(Type type)
            => this.GetService(type.FullName);
        #endregion

        #region Helper Methods
        /// <summary>
        /// Automatically generate all autobuild services.
        /// </summary>
        private void AutoBuildServices(ServiceLifetime lifetime)
        {
            this.root.services.Values.Where(d => d.AutoBuild && d.Lifetime == lifetime).ForEach(d =>
            {
                this.GetService(d.Id);
            });
        }

        /// <summary>
        /// Return a ServiceFactory instance from on factory
        /// type key.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ServiceFactory GetFactory(Type type)
            => _factories[type];

        /// <summary>
        /// Return the ServiceDescriptor instance with the
        /// given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceDescriptor GetServiceDescriptor(UInt32 id)
            => this.root.services[id];

        public ServiceDescriptor GetServiceDescriptor(String name)
            => this.GetServiceDescriptor(ServiceDescriptor.GetId(name));
        public ServiceDescriptor GetServiceDescriptor(Type type)
            => this.GetServiceDescriptor(type.FullName);

        public ServiceDescriptor GetServiceDescriptor<T>()
            => this.GetServiceDescriptor(typeof(T));

        /// <summary>
        /// Create a new scoped service provider instance.
        /// </summary>
        /// <returns></returns>
        public ServiceProvider CreateScope()
            => new ServiceProvider(this.root);

        /// <summary>
        /// If this is a scoped provider, return itself
        /// otherwise create and return a new scope.
        /// </summary>
        /// <returns></returns>
        public ServiceProvider GetScope()
            => this.root == this ? this.CreateScope() : this;
        #endregion
    }
}
