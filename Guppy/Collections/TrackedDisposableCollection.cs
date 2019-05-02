using Guppy.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class TrackedDisposableCollection<TTrackedDisposable> : IEnumerable
        where TTrackedDisposable : class, ITrackedDisposable
    {
        #region Protected Attributes
        protected List<TTrackedDisposable> list { get; private set; }
        #endregion

        #region Public Attributes
        public Boolean DisposeOnRemove { get; set; }
        #endregion

        #region Events
        public event EventHandler<TTrackedDisposable> Added;
        public event EventHandler<TTrackedDisposable> Removed;
        #endregion

        #region Constructors
        public TrackedDisposableCollection(Boolean disposeOnRemove = true)
        {
            this.list = new List<TTrackedDisposable>();

            this.DisposeOnRemove = disposeOnRemove;
        }
        #endregion

        #region Collection Methods
        public TTrackedDisposable ElementAt(Int32 index)
        {
            return this.list[index];
        }

        public virtual void Add(TTrackedDisposable item)
        {
            this.list.Add(item);

            item.Disposing += this.HandleDisposing;

            // Trigger the added event
            this.Added?.Invoke(this, item);
        }

        public virtual Boolean Remove(TTrackedDisposable item)
        {
            if(this.list.Remove(item))
            {
                item.Disposing -= this.HandleDisposing;

                if (this.DisposeOnRemove)
                    item.Dispose(); // Auto dispose the item if told to do so

                // Trigger the removed event
                this.Removed?.Invoke(this, item);

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

        public Int32 Count()
        {
            return this.list.Count;
        }
        #endregion

        #region Event Handlers
        private void HandleDisposing(object sender, ITrackedDisposable e)
        {
            // Auto remove the item when it gets disposed
            this.Remove(e as TTrackedDisposable);
        }
        #endregion

        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}
