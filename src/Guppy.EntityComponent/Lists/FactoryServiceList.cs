using Guppy.EntityComponent.Lists.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.EntityComponent.Interfaces;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.EntityComponent.Lists
{
    public class FactoryServiceList<TService> : ServiceList<TService>
        where TService : class, IService
    {
        #region Public Properties
        public String DefaultChildServiceName { get; set; }
        #endregion

        #region Events
        public new event ItemDelegate<TService> OnItemCreated
        {
            add => base.OnItemCreated += value;
            remove => base.OnItemCreated -= value;
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.DefaultChildServiceName = typeof(TService).FullName;
        }
        #endregion

        #region Create Methods
        public T Create<T>(
            String serviceName,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
                where T : class, TService
        {
            return this.Create<T>(this.provider, serviceName, customSetup, id);
        }
        public TService Create(
            String serviceName,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
        {
            return this.Create(this.provider, serviceName, customSetup, id);
        }
        public TService Create(
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
        {
            return this.Create(this.provider, customSetup, id);
        }

        public T Create<T>(
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
                where T : class, TService
        {
            return this.Create<T>(this.provider, customSetup, id);
        }
        public T Create<T>(
            String serviceName,
            Guid id)
                where T : class, TService
        {
            return this.Create<T>(this.provider, serviceName, id);
        }
        public TService Create(
            String serviceName,
            Guid id)
        {
            return this.Create(this.provider, serviceName, id);
        }
        public TService Create(
            Guid id)
        {
            return this.Create<TService>(this.provider, id);
        }

        public T Create<T>(
            Guid id)
                where T : class, TService
        {
            return this.Create<T>(this.provider, id);
        }

        public T Create<T>(
            String serviceName,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup)
                where T : class, TService
        {
            return this.Create<T>(this.provider, serviceName, customSetup);
        }
        public TService Create(
            String serviceName,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
        {
            return this.Create(this.provider, serviceName, customSetup);
        }
        public TService Create(
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
        {
            return this.Create(this.provider, customSetup);
        }

        public T Create<T>(
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
                where T : class, TService
        {
            return this.Create<T>(this.provider, customSetup);
        }

        public T Create<T>(
            String serviceName)
                where T : class, TService
        {
            return this.Create<T>(this.provider, serviceName);
        }
        public TService Create(
            String serviceName)
        {
            return this.Create(this.provider, serviceName);
        }
        public TService Create()
        {
            return this.Create<TService>(this.provider);
        }

        public T Create<T>()
                where T : class, TService
        {
            return this.Create<T>(this.provider);
        }
        #endregion

        #region GetOrCreateById Methods
        public T GetOrCreateById<T>(Guid id, String serviceName)
            where T : class, TService
                => this.GetById<T>(id) ?? this.Create<T>(serviceName, id);

        public TService GetOrCreateById(Guid id, String serviceName)
            => this.GetOrCreateById<TService>(id, serviceName);

        public T GetOrCreateById<T>(Guid id)
            where T : class, TService
                => this.GetById<T>(id) ?? this.Create<T>(id);

        public TService GetOrCreateById(Guid id)
            => this.GetOrCreateById<TService>(id);
        #endregion
    }
}
