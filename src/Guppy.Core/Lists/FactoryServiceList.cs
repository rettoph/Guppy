using Guppy.DependencyInjection;
using Guppy.Exceptions;
using Guppy.Interfaces;
using Guppy.Lists.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Lists
{
    public class FactoryServiceList<TService> : ServiceList<TService>
        where TService : class, IService
    {
        #region Public Properties
        public ServiceConfigurationKey DefaultChildServiceConfigurationKey { get; set; }
        #endregion

        #region Events
        public new event ItemDelegate<TService> OnCreated
        {
            add => base.OnCreated += value;
            remove => base.OnCreated -= value;
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.DefaultChildServiceConfigurationKey = ServiceConfigurationKey.From<TService>();
        }
        #endregion

        #region Create Methods
        public T Create<T>(ServiceConfigurationKey configurationKey, Action<T, ServiceProvider, ServiceConfiguration> setup = null, Guid? id = null)
            where T : class, TService
                => this.Create<T>(this.provider, configurationKey, setup, id);

        public TService Create(ServiceConfigurationKey configurationKey, Action<TService, ServiceProvider, ServiceConfiguration> setup = null, Guid? id = null)
                => this.Create<TService>(this.provider, configurationKey, setup, id);

        public T Create<T>(Action<T, ServiceProvider, ServiceConfiguration> setup = null, Guid? id = null)
            where T : class, TService
                => this.Create<T>(this.provider, ServiceConfigurationKey.From<T>(), setup, id);

        public TService Create(Action<TService, ServiceProvider, ServiceConfiguration> setup = null, Guid? id = null)
                => this.Create<TService>(this.provider, this.DefaultChildServiceConfigurationKey, setup, id);
        #endregion

        #region GetOrCreateById Methods
        public T GetOrCreateById<T>(Guid id, ServiceConfigurationKey configurationKey)
            where T : class, TService
                => this.GetById<T>(id) ?? this.Create<T>(configurationKey, null, id);

        public TService GetOrCreateById(Guid id, ServiceConfigurationKey configurationKey)
            => this.GetOrCreateById<TService>(id, configurationKey);

        public T GetOrCreateById<T>(Guid id)
            where T : class, TService
                => this.GetById<T>(id) ?? this.Create<T>(null, id);

        public TService GetOrCreateById(Guid id)
            => this.GetOrCreateById<TService>(id);
        #endregion
    }
}
