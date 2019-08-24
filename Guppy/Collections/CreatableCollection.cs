using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections;
using Guppy.Extensions.Collection;

namespace Guppy.Collections
{
    /// <summary>
    /// Contains a collecion of creatable objects
    /// removes the objects when they are disposed.
    /// </summary>
    /// <typeparam name="TResusable"></typeparam>
    public class CreatableCollection<TCreateable> : ISet<TCreateable>, IDisposable
        where TCreateable : Creatable
    {
        #region Private Fields
        private Dictionary<Guid, TCreateable> _idTable;
        private HashSet<TCreateable> _list;
        #endregion

        #region Protected Fields
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public EventDelegater Events { get; private set; }

        public Int32 Count
        {
            get { return _list.Count; }
        }

        public Boolean IsReadOnly { get; private set; } = false;
        #endregion

        #region Constructors
        public CreatableCollection(IServiceProvider provider)
        {
            _idTable = new Dictionary<Guid, TCreateable>();
            _list = new HashSet<TCreateable>();

            this.logger = provider.GetService<ILogger>();
            this.logger.LogTrace($"Created new CreatableCollection<{typeof(TCreateable).Name}> instance.");

            this.Events = provider.GetService<EventDelegater>();
            this.Events.TryRegister<TCreateable>("added");
            this.Events.TryRegister<TCreateable>("removed");
        }
        #endregion

        #region Lifecycle Methods
        public virtual void Dispose()
        {
            // auto dispose children
            while (this.Count > 0)
                this.First().Dispose();

            _idTable.Clear();
            _list.Clear();
        }
        #endregion

        #region Frame Methods

        #endregion

        #region Collection Methods
        /// <inheritdoc />
        public virtual Boolean Add(TCreateable item)
        {
            if (_list.Add(item))
            {
                _idTable.Add(item.Id, item);

                this.Events.Invoke<TCreateable>("added", this, item);

                item.Events.Add<Creatable>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual Boolean Remove(TCreateable item)
        {
            if (_list.Remove(item))
            {
                _idTable.Remove(item.Id);

                this.Events.Invoke<TCreateable>("removed", this, item);

                item.Events.Remove<Creatable>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<TCreateable> range)
        {
            foreach (TCreateable frameable in range)
                this.Add(frameable);
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            while (this.Count() > 0)
                this.Remove(this.ElementAt(0));
        }

        /// <inheritdoc />
        public void ExceptWith(IEnumerable<TCreateable> other)
        {
            other.ForEach(item => this.Remove(item));
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<TCreateable> other)
        {
            this.Clear();
            this.AddRange(other);
        }

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<TCreateable> other)
        {
            return _list.IsProperSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<TCreateable> other)
        {
            return _list.IsProperSupersetOf(other);
        }

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<TCreateable> other)
        {
            return _list.IsSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<TCreateable> other)
        {
            return _list.IsSupersetOf(other);
        }

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<TCreateable> other)
        {
            return _list.Overlaps(other);
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<TCreateable> other)
        {
            return other == this;
        }

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<TCreateable> other)
        {
            other.ForEach(item =>
            {
                if (this.Contains(item))
                    this.Remove(item);
                else
                    this.Add(item);
            });
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<TCreateable> other)
        {
            other.ForEach(item => this.Add(item));
        }

        /// <inheritdoc />
        void ICollection<TCreateable>.Add(TCreateable item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Contains(TCreateable item)
        {
            return _list.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(TCreateable[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<TCreateable> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region Helper Methods
        public Object GetById(Guid id)
        {
            if (_idTable.ContainsKey(id))
                return _idTable[id];

            return default(TCreateable);
        }

        public T GetById<T>(Guid id)
        {
            return (T)this.GetById(id);
        }
        #endregion

        #region Event Handlers
        private void HandleItemDisposing(object sender, Creatable arg)
        {
            // Auto remove the child on dispose
            this.Remove(sender as TCreateable);
        }
        #endregion
    }
}
