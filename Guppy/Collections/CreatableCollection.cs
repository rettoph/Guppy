using Guppy.Implementations;
using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Guppy.Collections
{
    /// <summary>
    /// Contains a collecion of creatable objects
    /// removes the objects when they are disposed.
    /// </summary>
    /// <typeparam name="TResusable"></typeparam>
    public class CreatableCollection<TCreateable> : HashSet<TCreateable>, IDisposable
        where TCreateable : Creatable
    {
        #region Private Fields
        private Dictionary<Guid, TCreateable> _idTable;
        #endregion

        #region Protected Attributes
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public EventDelegater Events { get; private set; }
        #endregion

        #region Constructors
        public CreatableCollection(IServiceProvider provider)
        {
            _idTable = new Dictionary<Guid, TCreateable>();

            this.logger = provider.GetService<ILogger>();
            this.Events = provider.GetService<EventDelegater>();

            this.Events.TryRegister<TCreateable>("added");
            this.Events.TryRegister<TCreateable>("removed");
        }
        #endregion

        #region Lifecycle Methods
        public void Dispose()
        {
            // auto dispose children
            while (this.Count > 0)
                this.First().Dispose();
        }
        #endregion

        #region Frame Methods

        #endregion

        #region Collection Methods
        public virtual new Boolean Add(TCreateable item)
        {
            if (base.Add(item))
            {
                _idTable.Add(item.Id, item);

                this.Events.Invoke<TCreateable>("added", this, item);

                item.Events.Add<Creatable>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        public virtual new Boolean Remove(TCreateable item)
        {
            if (base.Remove(item))
            {
                _idTable.Remove(item.Id);

                this.Events.Invoke<TCreateable>("removed", this, item);

                item.Events.Remove<Creatable>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        public virtual void AddRange(IEnumerable<TCreateable> range)
        {
            foreach (TCreateable frameable in range)
                this.Add(frameable);
        }

        public virtual new void Clear()
        {
            while (this.Count() > 0)
                this.Remove(this.ElementAt(0));
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
