using Guppy.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    public class ProtectedServiceCollection<TService> : Service, IEnumerable<TService>
        where TService : IService
    {
        #region Private Fields
        private Dictionary<Guid, TService> _dictionary;
        private List<TService> _list;
        #endregion

        #region Events
        protected delegate Boolean CheckItemDelegate(TService item);
        protected delegate void ItemDelegate(TService item);

        /// <summary>
        /// Recursive chain of delegates used to detect if a
        /// requested item may be added into the current collection.
        /// 
        /// Simple return true or false.
        /// </summary>
        protected event CheckItemDelegate OnCanAdd;

        /// <summary>
        /// Recursive chain of delegates used to detect if a
        /// requested item may be removed from the current collection.
        /// 
        /// Simply return true or false.
        /// </summary>
        protected event CheckItemDelegate OnCanRemove;

        /// <summary>
        /// Internal delegate invoked when an item is added.
        /// This should be used internally to finish configuring
        /// things as needed.
        /// </summary>
        protected event ItemDelegate OnAdd;

        /// <summary>
        /// Internal delegate invoked when an item is removed.
        /// This should be used internally to finish configuring
        /// things as needed.
        /// </summary>
        protected event ItemDelegate OnRemove;

        /// <summary>
        /// Public event invoked when an item is removed and 
        /// all internal configuration is complete.
        /// </summary>
        public event GuppyEventHandler<IEnumerable<TService>, TService> OnAdded;

        /// <summary>
        /// Public event invoked when an item is added and
        /// all internal configuration is complete
        /// </summary>
        public event GuppyEventHandler<IEnumerable<TService>, TService> OnRemoved;
        #endregion

        #region Constructor
        public ProtectedServiceCollection()
        {
            _dictionary = new Dictionary<Guid, TService>();
            _list = new List<TService>();
        }
        #endregion

        #region Lifecycle Methods
        protected override void Dispose()
        {
            base.Dispose();

            this.Clear();
        }
        #endregion

        #region Helper Methods
        protected Boolean CanAdd(TService item)
        {
            if (this.OnCanAdd != default(CheckItemDelegate))
                foreach (CheckItemDelegate checker in this.OnCanAdd.GetInvocationList())
                    if (!checker(item))
                        return false;

            return !_dictionary.ContainsKey(item.Id);
        }

        protected Boolean CanRemove(TService item)
        {
            if (this.OnCanRemove != default(CheckItemDelegate))
                foreach (CheckItemDelegate checker in this.OnCanRemove.GetInvocationList())
                    if (!checker(item))
                        return false;

            return _dictionary.ContainsKey(item.Id);
        }

        public TService GetById(Guid id)
        {
            if (_dictionary.ContainsKey(id))
                return _dictionary[id];

            return default(TService);
        }
        public T GetById<T>(Guid id)
            where T : TService
                => (T)this.GetById(id);

        public Int32 Count => _list.Count;

        protected Boolean Add(TService item)
        {
            if (this.CanAdd(item))
            {
                _list.Add(item);
                _dictionary.Add(item.Id, item);

                item.OnDisposed += this.HandleItemDisposed;

                this.OnAdd?.Invoke(item);
                this.OnAdded?.Invoke(this, item);

                return true;
            }

            return false;
        }

        protected Boolean Remove(TService item)
        {
            if (this.CanRemove(item))
            {
                _list.Remove(item);
                _dictionary.Remove(item.Id);

                item.OnDisposed -= this.HandleItemDisposed;

                this.OnRemove?.Invoke(item);
                this.OnRemoved?.Invoke(this, item);

                return true;
            }

            return false;
        }
        protected void Clear()
        {
            while (this.Count > 0) // Auto remove all elements
                this.Remove(_list.First());
        }
        #endregion

        #region IEnumerable Implementation
        public bool Contains(TService item)
            => _list.Contains(item);
        public bool ContainsId(Guid id)
            => _dictionary.ContainsKey(id);

        public IEnumerator<TService> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();
        #endregion

        #region Event Handlers
        private void HandleItemDisposed(IService sender)
        {
            this.Remove((TService)sender);
        }
        #endregion
    }
}
