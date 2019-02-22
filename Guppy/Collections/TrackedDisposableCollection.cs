using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class TrackedDisposableCollection<TTrackedDisposable>
        where TTrackedDisposable : class, ITrackedDisposable
    {
        #region Protected Attributes
        protected List<TTrackedDisposable> list { get; private set; }
        #endregion

        #region Constructors
        public TrackedDisposableCollection()
        {
            this.list = new List<TTrackedDisposable>();
        }
        #endregion

        #region Collection Methods
        public virtual void Add(TTrackedDisposable item)
        {
            this.list.Add(item);

            item.Disposing += this.HandleDisposing;
        }

        public virtual Boolean Remove(TTrackedDisposable item)
        {
            if(this.list.Remove(item))
            {
                item.Disposing -= this.HandleDisposing;

                return true;
            }

            return false;
        }

        public virtual void Clear()
        {
            while (this.list.Count > 0)
                this.Remove(this.list[0]);
        }

        public virtual Boolean Contains(TTrackedDisposable item)
        {
            return this.list.Contains(item);
        }
        #endregion

        #region Event Handlers
        private void HandleDisposing(object sender, ITrackedDisposable e)
        {
            // Auto remove the item when it gets disposed
            this.Remove(e as TTrackedDisposable);
        }
        #endregion
    }
}
