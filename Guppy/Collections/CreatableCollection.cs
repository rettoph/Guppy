using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections;
using Guppy.Extensions.Collection;
using Guppy.Utilities;
using Guppy.Utilities.Delegaters;
using System.Collections.Concurrent;
using Guppy.Interfaces;

namespace Guppy.Collections
{
    /// <summary>
    /// Contains a collecion of creatable objects
    /// removes the objects when they are disposed.
    /// </summary>
    /// <typeparam name="TResusable"></typeparam>
    public class CreatableCollection<TCreateable> : IEnumerable<TCreateable>, IDisposable
        where TCreateable : ICreatable
    {
        #region Private Fields
        private ConcurrentDictionary<Guid, TCreateable> _items;
        private TCreateable _item;
        #endregion

        #region Protected Fields
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public Int32 Count
        {
            get { return _items.Count; }
        }
        #endregion

        #region Events & Delegaters
        public event EventHandler<TCreateable> OnAdded;
        public event EventHandler<TCreateable> OnRemoved;
        #endregion

        #region Constructors
        public CreatableCollection(IServiceProvider provider)
        {
            _items = new ConcurrentDictionary<Guid, TCreateable>();

            this.logger = provider.GetService<ILogger>();
        }
        #endregion

        #region Lifecycle Methods
        public virtual void Dispose()
        {
            // auto dispose children
            while (this.Count > 0)
                this.First().Dispose();

            _items.Clear();
        }
        #endregion

        #region Collection Methods
        /// <inheritdoc />
        public virtual Boolean Add(TCreateable item)
        {
            if (_items.TryAdd(item.Id, item))
            {
                item.OnDisposing += this.HandleItemDisposing;

                this.OnAdded?.Invoke(this, item);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual Boolean Remove(TCreateable item)
        {
            if (_items.TryRemove(item.Id, out _item))
            {
                item.OnDisposing -= this.HandleItemDisposing;

                this.OnRemoved?.Invoke(this, item);

                return true;
            }

            return false;
        }

        public void Clear()
        {
            this.Remove(_items.First().Value);
        }

        public IEnumerator<TCreateable> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        #endregion

        #region Helper Methods
        public virtual TCreateable GetById(Guid id)
        {
            if (_items.TryGetValue(id, out _item))
                return _item;

            return default(TCreateable);
        }

        public virtual T GetById<T>(Guid id)
            where T : class, TCreateable
        {
            return this.GetById(id) as T;
        }
        #endregion

        #region Event Handlers
        private void HandleItemDisposing(object sender, EventArgs arg)
        {
            // Auto remove the child on dispose
            this.Remove((TCreateable)sender);
        }
        #endregion
    }
}
