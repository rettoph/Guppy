using Guppy.DependencyInjection;
using Guppy.Exceptions;
using Guppy.Interfaces;
using Guppy.Lists.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using DotNetUtils.DependencyInjection;

namespace Guppy.Lists
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
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.DefaultChildServiceName = typeof(TService).FullName;
        }
        #endregion

        #region CreateItem Methods
        public T CreateItem<T>(
            String serviceName,
            Action<T, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup,
            Guid id)
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider, serviceName, customSetup, id);
        }
        public virtual TService CreateItem(
            String serviceName,
            Action<TService, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup,
            Guid id)
        {
            return base.CreateItem(this.provider, serviceName, customSetup, id);
        }
        public virtual TService CreateItem(
            Action<TService, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup,
            Guid id)
        {
            return base.CreateItem(this.provider, customSetup, id);
        }

        public virtual T CreateItem<T>(
            Action<TService, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup,
            Guid id)
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider, customSetup, id);
        }
        public virtual T CreateItem<T>(
            String serviceName,
            Guid id)
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider, serviceName, id);
        }
        public virtual TService CreateItem(
            String serviceName,
            Guid id)
        {
            return base.CreateItem(this.provider, serviceName, id);
        }
        public virtual TService CreateItem(
            Guid id)
        {
            return base.CreateItem<TService>(this.provider, id);
        }

        public virtual T CreateItem<T>(
            Guid id)
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider, id);
        }

        public virtual T CreateItem<T>(
            String serviceName,
            Action<T, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup)
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider, serviceName, customSetup);
        }
        public virtual TService CreateItem(
            String serviceName,
            Action<TService, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup)
        {
            return base.CreateItem(this.provider, serviceName, customSetup);
        }
        public virtual TService CreateItem(
            Action<TService, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup)
        {
            return base.CreateItem(this.provider, customSetup);
        }

        public virtual T CreateItem<T>(
            Action<TService, GuppyServiceProvider, ServiceConfiguration<GuppyServiceProvider>> customSetup)
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider, customSetup);
        }

        public virtual T CreateItem<T>(
            String serviceName)
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider, serviceName);
        }
        public virtual TService CreateItem(
            String serviceName)
        {
            return base.CreateItem(this.provider, serviceName);
        }
        public virtual TService CreateItem()
        {
            return base.CreateItem(this.provider);
        }

        public virtual T CreateItem<T>()
                where T : class, TService
        {
            return base.CreateItem<T>(this.provider);
        }
        #endregion

        #region GetOrCreateById Methods
        public T GetOrCreateItemById<T>(Guid id, String serviceName)
            where T : class, TService
                => this.GetById<T>(id) ?? this.CreateItem<T>(serviceName, id);

        public TService GetOrCreateById(Guid id, String serviceName)
            => this.GetOrCreateItemById<TService>(id, serviceName);

        public T GetOrCreateById<T>(Guid id)
            where T : class, TService
                => this.GetById<T>(id) ?? this.CreateItem<T>(id);

        public TService GetOrCreateById(Guid id)
            => this.GetOrCreateById<TService>(id);
        #endregion
    }
}
