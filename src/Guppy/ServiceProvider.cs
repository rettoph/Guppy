using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy
{
    public sealed class ServiceProvider : Service, IServiceProvider
    {
        #region Private Fields
        /// <summary>
        /// Collection of configured services & their descriptors
        /// indexed by their id.
        /// </summary>
        private Dictionary<UInt32, ServiceDescriptor> _services;

        /// <summary>
        /// Custom factory methods. If not defined, 
        /// System.Activator will automatically be used.
        /// </summary>
        private Dictionary<Type, Func<ServiceProvider, Type, Object>> _factories;
        #endregion

        #region Internal Fields
        /// <summary>
        /// Cache of scoped instances by id.
        /// </summary>
        internal Dictionary<UInt32, IService> scopes;
        /// <summary>
        /// Cache of singleton instances by id.
        /// </summary>
        internal Dictionary<UInt32, IService> singletons;

        /// <summary>
        /// Cache of scoped instances by type.
        /// </summary>
        internal Dictionary<Type, IService> typedScopes;
        /// <summary>
        /// Cache of singleton instances by type.
        /// </summary>
        internal Dictionary<Type, IService> typedSingletons;

        /// <summary>
        /// Cache of generic (non IService) singleton instances.
        /// </summary>
        internal Dictionary<Type, Object> genericSingletons;
        #endregion

        #region Constructor
        internal ServiceProvider(ServiceCollection collection)
        {
            _factories = collection.factories;
            _services = collection.ToDictionary(
                keySelector: i => i.Id,
                elementSelector: i => i);

            this.singletons = new Dictionary<UInt32, IService>();
            this.typedSingletons = collection.typedSingletons;
            this.genericSingletons = collection.genericSingletons;
            this.scopes = new Dictionary<UInt32, IService>();
            this.typedScopes = new Dictionary<Type, IService>();
        }

        private ServiceProvider(ServiceProvider parent)
        {
            _factories = parent._factories;
            _services = parent._services;

            this.singletons = parent.singletons;
            this.typedSingletons = parent.typedSingletons;
            this.genericSingletons = parent.genericSingletons;

            this.scopes = new Dictionary<UInt32, IService>(parent.scopes);
            this.typedScopes = new Dictionary<Type, IService>(parent.typedScopes);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Get a requested service by its UInt32 Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public IService GetService(UInt32 id, Action<ServiceProvider, IService> setup = null)
        {
            return _services[id].GetInstance(this, setup);
        }

        /// <summary>
        /// Get a requested service by its String Handle.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public IService GetService(String handle, Action<ServiceProvider, IService> setup = null)
        {
            return this.GetService(xxHash.CalculateHash(Encoding.UTF8.GetBytes(handle)), setup);
        }

        /// <summary>
        /// Get a requested service by its Type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public Object GetService(Type serviceType, Action<ServiceProvider, IService> setup)
        {
            ExceptionHelper.ValidateAssignableFrom<IService>(serviceType);

            return this.GetService(serviceType.FullName, setup);
        }

        /// <summary>
        /// Create a new scoped ServiceProvider instance.
        /// </summary>
        /// <returns></returns>
        public ServiceProvider CreateScope()
        {
            return new ServiceProvider(this);
        }

        /// <summary>
        /// Auto generate an instance of the requested type, assuming it is
        /// an implementation of IService
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        internal IService BuildInstance(Type serviceType)
        {
            // Make sure a valid service is revieved.
            ExceptionHelper.ValidateAssignableFrom<IService>(serviceType);

            if (serviceType.IsAbstract)
                throw new ArgumentException("Requested type is abstract.");
            else if (!serviceType.IsClass)
                throw new ArgumentException("Requested type is not class.");
            else if (_factories.ContainsKey(serviceType))
                return (IService)_factories[serviceType](this, serviceType);
            else
                return (IService)Activator.CreateInstance(serviceType);
        }
        #endregion

        #region IServiceProvider Implmentation
        public Object GetService(Type serviceType)
        {
            if (typeof(IService).IsAssignableFrom(serviceType))
                return this.GetService(serviceType.FullName);
            else if (this.genericSingletons.ContainsKey(serviceType))
                return this.genericSingletons[serviceType];
            else if (_factories.ContainsKey(serviceType))
                return _factories[serviceType](this, serviceType);
            else
                throw new ArgumentException();
        }
        #endregion

    }
}
