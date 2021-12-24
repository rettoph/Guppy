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
            Action<T, ServiceProvider, ServiceConfiguration> customSetup)
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
    }
}
