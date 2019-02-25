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

        #region Public Attributes
        public Boolean DisposeOnRemove { get; set; }
        #endregion

        #region Constructors
        public TrackedDisposableCollection(Boolean disposeOnRemove = true)
        {
            this.list = new List<TTrackedDisposable>();

            this.DisposeOnRemove = disposeOnRemove;
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

                if (this.DisposeOnRemove)
                    item.Dispose(); // Auto dispose the item if told to do so

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
