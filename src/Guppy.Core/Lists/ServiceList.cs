using Guppy.Events;
using Guppy.Events.Delegates;
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

namespace Guppy.Lists
{
    /// <summary>
    /// An collection that contained TService instances.
    /// When the service provider created new TService 
    /// instances they will automatically be placed into 
    /// the current collection.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public partial class ServiceList<TService> : Service, IServiceList<TService>
        where TService : class, IService
    {
        #region Private Fields
        private Dictionary<Guid, TService> _dictionary;
        private List<TService> _list;
        private Stack<TService> _created;
        #endregion

        #region Protected Fields
        protected ServiceProvider provider { get; private set; }
        #endregion

        #region Public Properties
        public Int32 Count => _list.Count;

        Type IServiceList.BaseType => typeof(TService);
        ServiceProvider IServiceList.Provider => this.provider;
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
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            _dictionary = new Dictionary<Guid, TService>();
            _list = new List<TService>();
            _created = new Stack<TService>();
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.provider = provider;

            this.CanAdd += this.HandleCanAdd;
            this.OnAdd += this.HandleAdd;

            this.CanRemove += this.HandleCanRemove;
            this.OnRemove += this.HandleRemove;
        }

        protected override void Release()
        {
            base.Release();

            this.CanAdd -= this.HandleCanAdd;
            this.OnAdd -= this.HandleAdd;

            this.CanRemove -= this.HandleCanRemove;
            this.OnRemove -= this.HandleRemove;

            _dictionary.Clear();
            _list.Clear();
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
                => (_dictionary.ContainsKey(id) ? _dictionary[id] : _created.FirstOrDefault(i => i.Id == id)) as T;

        protected void Clear()
        {
            while (this.Any()) // Auto remove all elements
                this.TryRemove(_list.First());
        }
        #endregion

        #region Event Handlers
        private Boolean HandleCanAdd(IServiceList<TService> sender, TService item)
            => !_dictionary.ContainsKey(item.Id);

        private void HandleAdd(TService item)
        {
            _list.Add(item);
            _dictionary.Add(item.Id, item);

            item.OnReleased += this.HandleItemReleased;
        }

        private Boolean HandleCanRemove(IServiceList<TService> sender, TService item)
            => _dictionary.ContainsKey(item.Id);

        private void HandleRemove(TService item)
        {
            _list.Remove(item);
            _dictionary.Remove(item.Id);

            item.OnReleased -= this.HandleItemReleased;
        }

        private void HandleItemReleased(IService sender)
        {
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
            ExceptionHelper.ValidateAssignableFrom<TService>(instance.GetType());

            this.TryAdd(instance as TService);
        }

        T IServiceList.GetById<T>(Guid id)
        {
            if (_dictionary.ContainsKey(id))
                return _dictionary[id] as T;

            return default(T);
        }
        #endregion
    }

    public class ServiceList : ServiceList<IService>
    {
    }
}
