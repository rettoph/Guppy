using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Lists.Interfaces;
using Guppy.Lists.Delegates;
using Guppy.Enums;
using Guppy.Extensions.DependencyInjection;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.Interfaces;

namespace Guppy.Lists
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
        protected GuppyServiceProvider provider { get; private set; }

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
        GuppyServiceProvider IServiceList.Provider => this.provider;
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

        protected event ItemDelegate<TService> OnCreated;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            _dictionary = new Dictionary<Guid, TService>();
            _list = new List<TService>();
            _creating = new List<TService>();
        }

        protected override void PreInitialize(GuppyServiceProvider provider)
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

        public TService GetById(Guid id)
            => this.GetById<TService>(id);

        public virtual T GetById<T>(Guid id)
            where T : class, TService
                => (_dictionary.ContainsKey(id) ? _dictionary[id] : default) as T;

        public void Clear()
        {
            while (this.Any()) // Auto remove all elements
                this.TryRemove(_list.First());
        }

        protected virtual T Create<T>(
            GuppyServiceProvider provider,
            ServiceConfigurationKey configurationKey,
            Action<T, GuppyServiceProvider, IServiceConfiguration> setup = null,
            Guid? id = null)
            where T : class, TService
        {
            var instance = provider.GetService<T>(configurationKey, (i, p, d) =>
            {
                if (id.HasValue)
                    i.Id = id.Value;

                // TODO: REMOVE NULL CHECK
                setup?.Invoke(i, p, d);
                _creating.Add(i);

                this.OnCreated?.Invoke(i);
            });

            this.TryAdd(instance);
            _creating.Remove(instance);

            return instance;
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
