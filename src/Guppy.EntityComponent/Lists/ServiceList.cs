using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.EntityComponent.Lists.Interfaces;
using Guppy.EntityComponent.Lists.Delegates;
using Guppy.EntityComponent.Interfaces;
using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;

namespace Guppy.EntityComponent.Lists
{
    /// <summary>
    /// An collection that contained TService instances.
    /// When the service provider created new TService 
    /// instances they will automatically be placed into 
    /// the current collection.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public partial class ServiceList<TService> : Entity, IServiceList<TService>
        where TService : class, IService
    {
        #region Private Fields
        private Dictionary<Guid, TService> _dictionary;
        private List<TService> _list;
        private List<TService> _creating;
        #endregion

        #region Protected Fields
        protected ServiceProvider provider { get; private set; }

        /// <summary>
        /// When true, then all self contained items
        /// will be released with the invocation of
        /// <see cref="Release"/>. Otherwise, the list
        /// will only be cleared.
        /// </summary>
        protected Boolean releaseChildren { get; set; } = false;
        #endregion

        #region Public Properties
        public Int32 Count => _list.Count;

        Type IServiceList.BaseType => typeof(TService);
        ServiceProvider IServiceList.Provider => this.provider;

        public TService this[Guid id] => this.GetById(id);
        #endregion

        #region Events
        /// <inheritdoc />
        public event ValidateEventDelegate<IServiceList<TService>, TService> CanAdd;

        /// <inheritdoc />
        public event ValidateEventDelegate<IServiceList<TService>, TService> CanRemove;

        /// <summary>
        /// Internal delegate invoked when an item is being added.
        /// This should be used internally to finish configuring
        /// things as needed.
        /// </summary>
        protected event ItemDelegate<TService> OnAdd;

        /// <summary>
        /// Internal delegate invoked when an item is being removed.
        /// This should be used internally to finish configuring
        /// things as needed.
        /// </summary>
        protected event ItemDelegate<TService> OnRemove;

        /// <inheritdoc />
        public event OnEventDelegate<IServiceList<TService>, TService> OnAdded;

        /// <inheritdoc />
        public event OnEventDelegate<IServiceList<TService>, TService> OnRemoved;

        /// <summary>
        /// Simple event invoked when an item is created.
        /// </summary>
        protected event ItemDelegate<TService> OnItemCreated;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            _dictionary = new Dictionary<Guid, TService>();
            _list = new List<TService>();
            _creating = new List<TService>();
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.provider = provider;
            this.releaseChildren = false;

            this.CanAdd += this.HandleCanAdd;
            this.OnAdd += this.HandleAdd;

            this.CanRemove += this.HandleCanRemove;
            this.OnRemove += this.HandleRemove;
        }

        protected override void Release()
        {
            base.Release();

            if(this.releaseChildren)
            { // Auto release all children.
                while (this.Any())
                    this.First().TryRelease();
            }
            else
            { // Simply clear all children.
                this.Clear();
            }


            this.CanAdd -= this.HandleCanAdd;
            this.OnAdd -= this.HandleAdd;

            this.CanRemove -= this.HandleCanRemove;
            this.OnRemove -= this.HandleRemove;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.provider = null;
        }
        #endregion

        #region Helper Methods
        public Boolean TryAdd(TService item)
        {
            if (!this.CanAdd.Validate(this, item, false))
                return false;

            this.OnAdd?.Invoke(item);
            this.OnAdded?.Invoke(this, item);

            return true;
        }

        public Boolean TryRemove(TService item)
        {
            if (!this.CanRemove.Validate(this, item, false))
                return false;

            this.OnRemove?.Invoke(item);
            this.OnRemoved?.Invoke(this, item);

            return true;
        }

        public Boolean Contains(TService item)
            => _dictionary.ContainsKey(item.Id);

        public virtual Boolean TryGetById(Guid id, out TService item)
            => _dictionary.TryGetValue(id, out item);

        public virtual Boolean TryGetById<T>(Guid id, out T item)
            where T : class, TService
        {
            if(this.TryGetById(id, out TService source))
            {
                item = source as T;
                return true;
            }

            item = default;
            return false;
        }

        public virtual TService GetById(Guid id)
        {
            this.TryGetById(id, out TService item);
            return item;
        }

        public virtual T GetById<T>(Guid id)
            where T : class, TService
        {
            this.TryGetById(id, out T item);
            return item;
        }

        public void Clear()
        {
            while (this.Any()) // Auto remove all elements
                this.TryRemove(_list.First());
        }
        #endregion

        #region Create Methods
        protected virtual T Create<T>(
            ServiceProvider provider,
            String serviceName,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
                where T : class, TService
        {
            var instance = provider.GetService<T>(serviceName, (i, p, d) =>
            {
                i.Id = id;

                customSetup.Invoke(i, p, d);
                _creating.Add(i);

                this.OnItemCreated?.Invoke(i);
            });

            this.TryAdd(instance);
            _creating.Remove(instance);

            return instance;
        }
        protected virtual TService Create(
            ServiceProvider provider,
            String serviceName,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
        {
            return this.Create<TService>(provider, serviceName, customSetup, id);
        }
        protected TService Create(
            ServiceProvider provider,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
        {
            return this.Create<TService>(provider, typeof(TService).FullName, customSetup, id);
        }

        protected T Create<T>(
            ServiceProvider provider,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup,
            Guid id)
                where T : class, TService
        {
            return this.Create<T>(provider, typeof(T).FullName, customSetup, id);
        }
        protected virtual T Create<T>(
            ServiceProvider provider,
            String serviceName,
            Guid id)
                where T : class, TService
        {
            var instance = provider.GetService<T>(serviceName, (i, p, d) =>
            {
                i.Id = id;

                _creating.Add(i);

                this.OnItemCreated?.Invoke(i);
            });

            this.TryAdd(instance);
            _creating.Remove(instance);

            return instance;
        }
        protected TService Create(
            ServiceProvider provider,
            String serviceName,
            Guid id)
        {
            return this.Create<TService>(provider, serviceName, id);
        }
        protected TService Create(
            ServiceProvider provider,
            Guid id)
        {
            return this.Create<TService>(provider, typeof(TService).FullName, id);
        }

        protected T Create<T>(
            ServiceProvider provider,
            Guid id)
                where T : class, TService
        {
            return this.Create<T>(provider, typeof(T).FullName, id);
        }

        protected virtual T Create<T>(
            ServiceProvider provider,
            String serviceName,
            Action<T, ServiceProvider, ServiceConfiguration> customSetup)
                where T : class, TService
        {
            var instance = provider.GetService<T>(serviceName, (i, p, d) =>
            {
                customSetup.Invoke(i, p, d);
                _creating.Add(i);

                this.OnItemCreated?.Invoke(i);
            });

            this.TryAdd(instance);
            _creating.Remove(instance);

            return instance;
        }
        protected TService Create(
            ServiceProvider provider,
            String serviceName,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
        {
            return this.Create<TService>(provider, serviceName, customSetup);
        }
        protected TService Create(
            ServiceProvider provider,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
        {
            return this.Create<TService>(provider, typeof(TService).FullName, customSetup);
        }

        protected T Create<T>(
            ServiceProvider provider,
            Action<TService, ServiceProvider, ServiceConfiguration> customSetup)
                where T : class, TService
        {
            return this.Create<T>(provider, typeof(T).FullName, customSetup);
        }

        protected virtual T Create<T>(
            ServiceProvider provider,
            String serviceName)
                where T : class, TService
        {
            var instance = provider.GetService<T>(serviceName, (i, p, d) =>
            {
                _creating.Add(i);

                this.OnItemCreated?.Invoke(i);
            });

            this.TryAdd(instance);
            _creating.Remove(instance);

            return instance;
        }
        protected TService Create(
            ServiceProvider provider,
            String serviceName)
        {
            return this.Create<TService>(provider, serviceName);
        }

        protected T Create<T>(
            ServiceProvider provider)
                where T : class, TService
        {
            return this.Create<T>(provider, typeof(T).FullName);
        }
        #endregion

        #region Event Handlers
        private Boolean HandleCanAdd(IServiceList<TService> sender, TService item)
            => item != default && item.Status != ServiceStatus.NotInitialized && !_dictionary.ContainsKey(item.Id);

        private void HandleAdd(TService item)
        {
            _list.Add(item);
            _dictionary.Add(item.Id, item);

            item.OnStatusChanged += this.HandleItemStatusChanged;
        }

        private Boolean HandleCanRemove(IServiceList<TService> sender, TService item)
            => _dictionary.ContainsKey(item.Id);

        private void HandleRemove(TService item)
        {
            _list.Remove(item);
            _dictionary.Remove(item.Id);

            item.OnStatusChanged -= this.HandleItemStatusChanged;
        }

        protected virtual void HandleItemStatusChanged(IService sender, ServiceStatus old, ServiceStatus value)
        {
            if(value == ServiceStatus.PreReleasing)
                this.TryRemove(sender as TService);
        }
        #endregion

        #region IEnumerable<TService> Implementation
        public IEnumerator<TService> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();
        #endregion

        #region IServiceList Implementation
        void IServiceList.TryAdd(Object instance)
        {
            if(typeof(TService).ValidateAssignableFrom(instance.GetType()))
            {
                this.TryAdd(instance as TService);
            }
        }

        T IServiceList.GetById<T>(Guid id)
        {
            if (_dictionary.ContainsKey(id))
                return _dictionary[id] as T;

            return _creating.FirstOrDefault(i => i.Id == id) as T;
        }
        #endregion
    }

    public class ServiceList : ServiceList<IService>
    {
    }
}
