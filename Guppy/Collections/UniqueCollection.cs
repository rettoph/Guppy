using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Collections
{
    /// <summary>
    /// Contains a collecion of reusable objects and automatically
    /// removes the objects when they are disposed.
    /// </summary>
    /// <typeparam name="TResusable"></typeparam>
    public class UniqueCollection<TUnique> : HashSet<TUnique>, IDisposable
        where TUnique : class, IUnique
    {
        #region Protected Attributes
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public EventDelegater Events { get; private set; }
        #endregion

        #region Constructors
        public UniqueCollection(IServiceProvider provider)
        {
            this.logger = provider.GetService<ILogger>();
            this.Events = provider.GetService<EventDelegater>();

            this.Events.TryRegisterDelegate<TUnique>("added");
            this.Events.TryRegisterDelegate<TUnique>("removed");
        }
        #endregion

        #region Lifecycle Methods
        public void Dispose()
        {
            // auto dispose children
            while(this.Count > 0)
                this.First().Dispose();
        }
        #endregion

        #region Frame Methods

        #endregion

        #region Collection Methods
        public virtual new Boolean Add(TUnique item)
        {
            if (base.Add(item))
            {
                this.Events.Invoke<TUnique>("added", item);

                item.Events.AddDelegate<DateTime>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        public virtual new Boolean Remove(TUnique item)
        {
            if (base.Remove(item))
            {
                this.Events.Invoke<TUnique>("removed", item);

                item.Events.RemoveDelegate<DateTime>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        public virtual void AddRange(IEnumerable<TUnique> range)
        {
            foreach (TUnique frameable in range)
                this.Add(frameable);
        }
        #endregion

        #region Helper Methods
        public Object GetById(Guid id)
        {
            return this.First(u => u.Id == id);
        }

        public T GetById<T>(Guid id)
        {
            return (T)this.GetById(id);
        }
        #endregion

        #region Event Handlers
        private void HandleItemDisposing(object sender, DateTime arg)
        {
            // Auto remove the child on dispose
            this.Remove(sender as TUnique);
        }
        #endregion
    }
}
