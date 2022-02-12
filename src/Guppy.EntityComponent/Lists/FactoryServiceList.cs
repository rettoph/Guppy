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
        public new T Create<T>(
            Type type,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup)
                where T : class, TService
        {
            return base.Create<T>(type, customSetup);
        }
        public new TService Create(
            Type type,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
        {
            return base.Create(type, customSetup);
        }
        public new TService Create(
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
        {
            return base.Create(customSetup);
        }

        public new T Create<T>(
            Action<T, ServiceProvider, ServiceConfiguration> customSetup)
                where T : class, TService
        {
            return base.Create<T>(customSetup);
        }

        public new T Create<T>(
            Type type)
                where T : class, TService
        {
            return base.Create<T>(type);
        }
        public new TService Create(
            Type type)
        {
            return base.Create(type);
        }
        public TService Create()
        {
            return base.Create<TService>();
        }

        public new T Create<T>()
                where T : class, TService
        {
            return base.Create<T>();
        }
        #endregion
    }
}
