using Guppy.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceProvider : IServiceProvider
    {
        #region Private Fields
        /// <summary>
        /// A lookup table of all configured factories by type.
        /// </summary>
        private Dictionary<Type, ServiceFactory> _factories;

        /// <summary>
        /// A lookup table of all service descriptors by id.
        /// </summary>
        private Dictionary<UInt32, ServiceDescriptor> _services;

        private ServiceDescriptor _service;
        #endregion

        #region Internal Fields
        internal Dictionary<Type, Object> scopedInstances;
        internal Dictionary<Type, Object> singletonInstances;
        #endregion

        #region Public Fields
        public readonly Guid Id;
        #endregion

        #region Constructors
        internal ServiceProvider(ServiceCollection collection)
        {
            // Create a lookup table of service factories.
            _factories = collection.factories
                .GroupBy(fd => fd.Type)
                .Select(g => g.OrderBy(fd => fd.Priority).First())
                .Select(fd => new ServiceFactory(fd))
                .ToDictionary(f => f.Type);

            // Create a lookup table of service descriptors
            _services = collection.services
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
        }
        private ServiceProvider(ServiceProvider parent)
        {
            this.scopedInstances = new Dictionary<Type, Object>();
            this.singletonInstances = parent.singletonInstances;

            this.Id = Guid.NewGuid();
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
            try
            {
                _service = _services[id];
            }
            catch(KeyNotFoundException e)
            {
                throw new ServiceIdUnknownException(id);
            }

            return _service.GetInstance(this, setup);
        }

        /// <summary>
        /// Return a new instance of a service
        /// based on the recieved service name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public Object GetService(String name, Action<Object, ServiceProvider, ServiceDescriptor> setup = null)
        {
            try
            {
                return this.GetService(ServiceDescriptor.GetId(name), setup);
            }
            catch(ServiceIdUnknownException e)
            {
                throw new ServiceNameUnknown(name, e);
            }
        }

        /// <summary>
        /// Return a new nameless service instance
        /// based on the recieved type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public Object GetService(Type type, Action<Object, ServiceProvider, ServiceDescriptor> setup)
        {
            try
            {
                return this.GetService(type.FullName, setup);
            }
            catch (ServiceNameUnknown e)
            {
                throw new ServiceTypeUnknown(type, e);
            }
        }

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
            => _services[id];

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
            => new ServiceProvider(this);
        #endregion
    }
}
