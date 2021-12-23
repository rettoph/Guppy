using Guppy.EntityComponent.Utilities;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public partial class ServiceProvider : IDisposable
    {
        #region Private Fields
        /// <summary>
        /// All registered <see cref="ServiceConfiguration"/> instances.
        /// </summary>
        private DoubleDictionary<String, UInt32, ServiceConfiguration> _registeredServices;

        /// <summary>
        /// All <see cref="ServiceConfigurationManager"/> instances that have been activated
        /// for the current scope by id.
        /// </summary>
        private DoubleDictionary<String, UInt32, ServiceConfigurationManager> _activeServices;

        private Boolean _disposing;
        private ServiceProvider _parent;
        private List<ServiceProvider> _children;
        #endregion

        #region Public Fields
        /// <summary>
        /// Indicates that this provider is the Root most scope, from which all other scopes are children.
        /// When true, <see cref="SingletonServiceConfigurationManager"/> instances will all reside here.
        /// </summary>
        public readonly Boolean IsRoot;

        /// <summary>
        /// The rootmost ServiceProvider.
        /// </summary>
        public readonly ServiceProvider Root;

        /// <summary>
        /// Simple lookup table for all Entity ComponentConfigurations.
        /// </summary>

        public readonly Dictionary<UInt32, ComponentConfiguration[]> EntityComponentConfigurations;
        #endregion

        #region Public Properties
        public Settings Settings { get; private set; }
        #endregion

        #region Events
        public event OnEventDelegate<ServiceProvider, ServiceConfiguration> OnServiceActivated;
        #endregion

        #region Constructors
        internal ServiceProvider(
            DoubleDictionary<String, UInt32, ServiceConfiguration> serviceConfigurations,
            Dictionary<UInt32, ComponentConfiguration[]> entityComponentConfigurations)
        {
            this.IsRoot = true;
            this.Root = this;

            _registeredServices = serviceConfigurations;
            _activeServices = new DoubleDictionary<String, UInt32, ServiceConfigurationManager>();

            _children = new List<ServiceProvider>();

            this.EntityComponentConfigurations = entityComponentConfigurations;
            this.Settings = this.GetService<Settings>();
        }
        protected ServiceProvider(ServiceProvider parent)
        {
            this.IsRoot = false;
            this.Root = parent.Root;

            _registeredServices = parent._registeredServices;
            _activeServices = parent._activeServices.Where(kkvp => kkvp.value is SingletonServiceConfigurationManager).ToDoubleDictionary();

            _parent = parent;
            _children = new List<ServiceProvider>();

            this.EntityComponentConfigurations = parent.EntityComponentConfigurations;
            this.Settings = this.GetService<Settings>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Return the internal <see cref="ServiceConfigurationManager"/>, or create a new one
        /// if the <see cref="ServiceConfiguration"/> is not yet active.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal ServiceConfigurationManager GetServiceConfigurationManager(ServiceConfiguration configuration)
        {
            if(_activeServices.TryGetValue(configuration.Name, out ServiceConfigurationManager manager))
            {
                return manager;
            }

            return this.ActivateServiceConfiguration(configuration);
        }

        /// <summary>
        /// Activate a given <see cref="ServiceConfiguration"/>. This will create a new
        /// <see cref="ServiceConfigurationManager"/> instance and store its lookup type so
        /// that it will be utilized by all future queries of any 
        /// <see cref="ServiceConfiguration.CacheNames"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private ServiceConfigurationManager ActivateServiceConfiguration(ServiceConfiguration configuration)
        {
            ServiceConfigurationManager manager = configuration.BuildServiceCofigurationManager(this);

            // Cache all of the configurations CacheNames so that future lookup will return this instance.
            // If there is any overlap at all this will fail.
            foreach(String cacheName in configuration.CacheNames)
            {
                _activeServices.TryAdd(cacheName, cacheName.xxHash(), manager);
            }

            this.OnServiceActivated?.Invoke(this, configuration);

            return manager;
        }

        public virtual ServiceProvider CreateScope()
        {
            return new ServiceProvider(this);
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            if (_disposing)
                return;

            _disposing = true;

            this.Dispose(_disposing);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            foreach (ServiceProvider child in _children)
            {
                child.Dispose();
            }
                

            foreach (ServiceConfigurationManager manager in _activeServices.Values)
            {
                manager.Dispose();
            }

            _parent = default;
            _children.Clear();

            _activeServices.Clear();
            _registeredServices.Clear(); // Will this clear all scopes?
        }
        #endregion
    }
}
