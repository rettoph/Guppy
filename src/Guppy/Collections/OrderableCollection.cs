using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Simple collection used to contain several IOrderable insatnces.
    /// 
    /// This will automatically manage 2 lists of Visible/Enabled items
    /// and each frame will update both lists in Update/Draw order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderableCollection<T> : ServiceCollection<T>, IFrameable
        where T : Orderable
    {
        #region Private Attributes
        private List<T> _draws;
        private List<T> _updates;
        #endregion

        #region Protected Attributes
        protected Boolean dirtyDraws { get; set; }
        protected Boolean dirtyUpdates { get; set; }
        #endregion

        #region Public Attributes
        public IEnumerable<T> Draws { get { return _draws; } }
        public IEnumerable<T> Updates { get { return _updates; } }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _draws = new List<T>();
            _updates = new List<T>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;
        }
        #endregion

        #region Frame Methods
        public virtual void TryUpdate(GameTime gameTime)
        {
            this.TryCleanUpdates();

            _updates.ForEach(u => u.TryUpdate(gameTime));
        }

        public virtual void TryDraw(GameTime gameTime)
        {
            this.TryCleanDraws();

            _draws.ForEach(u => u.TryDraw(gameTime));
        }

        public virtual void TryCleanDraws()
        {
            if (this.dirtyDraws)
            {
                _draws.Clear();
                _draws.AddRange(this.RemapDraws());
                this.dirtyDraws = false;
            }
        }

        public virtual void TryCleanUpdates()
        {
            if (this.dirtyUpdates)
            {
                _updates.Clear();
                _updates.AddRange(this.RemapUpdates());
                this.dirtyUpdates = false;
            }
        }
        #endregion

        #region Collection Methods
        protected override void Clear()
        {
            base.Clear();

            _draws.Clear();
            _updates.Clear();
        }

        protected override void Add(T item)
        {
            base.Add(item);

            if (item.Visible)
                this.dirtyDraws = true;
            if (item.Enabled)
                this.dirtyUpdates = true;

            item.OnVisibleChanged += this.HandleItemVisibleChanged;
            item.OnEnabledChanged += this.HandleItemEnabledChanged;
            item.OnUpdateOrderChanged += this.HandleItemUpdateOrderChanged;
            item.OnDrawOrderChanged += this.HandleItemDrawOrderChanged;
        }

        protected override void Remove(T item)
        {
            base.Remove(item);

            if (item.Visible)
                this.dirtyDraws = true;
            if (item.Enabled)
                this.dirtyUpdates = true;

            item.OnVisibleChanged -= this.HandleItemVisibleChanged;
            item.OnEnabledChanged -= this.HandleItemEnabledChanged;
            item.OnUpdateOrderChanged -= this.HandleItemUpdateOrderChanged;
            item.OnDrawOrderChanged -= this.HandleItemDrawOrderChanged;
        }
        #endregion

        #region Helper Methods
        protected virtual IEnumerable<T> RemapDraws()
        {
            return this.Where(o => o.Visible).OrderBy(o => o.DrawOrder);
        }

        protected virtual IEnumerable<T> RemapUpdates()
        {
            return this.Where(o => o.Enabled).OrderBy(o => o.UpdateOrder);
        }
        #endregion

        #region Event Handlers
        private void HandleItemVisibleChanged(object sender, bool arg)
        {
            this.dirtyDraws = true;
        }

        private void HandleItemEnabledChanged(object sender, bool arg)
        {
            this.dirtyUpdates = true;
        }

        private void HandleItemDrawOrderChanged(object sender, int e)
        {
            this.dirtyDraws = true;
        }

        private void HandleItemUpdateOrderChanged(object sender, int e)
        {
            this.dirtyUpdates = true;
        }
        #endregion
    }
}
