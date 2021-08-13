using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Lists
{
    public class OrderableList<TOrderable> : FrameableList<TOrderable>
        where TOrderable : class, IOrderable
    {
        #region Private Attributes
        private List<TOrderable> _draws;
        private List<TOrderable> _updates;
        #endregion

        #region Protected Attributes
        protected Boolean dirtyDraws { get; set; }
        protected Boolean dirtyUpdates { get; set; }
        #endregion

        #region Public Attributes
        public IEnumerable<TOrderable> Draws { get { return _draws; } }
        public IEnumerable<TOrderable> Updates { get { return _updates; } }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnAdd += this.AddItem;
            this.OnRemove += this.RemoveItem;
        }

        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            _draws = new List<TOrderable>();
            _updates = new List<TOrderable>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;
        }

        protected override void Release()
        {
            base.Release();

            _draws.Clear();
            _updates.Clear();

            this.OnAdd -= this.AddItem;
            this.OnRemove -= this.RemoveItem;
        }
        #endregion

        #region Collection Methods
        private void AddItem(TOrderable item)
        {
            if (item.Visible)
                this.dirtyDraws = true;
            if (item.Enabled)
                this.dirtyUpdates = true;

            item.OnVisibleChanged += this.HandleItemVisibleChanged;
            item.OnEnabledChanged += this.HandleItemEnabledChanged;
            item.OnUpdateOrderChanged += this.HandleItemUpdateOrderChanged;
            item.OnDrawOrderChanged += this.HandleItemDrawOrderChanged;
        }

        private void RemoveItem(TOrderable item)
        {

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

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            this.TryCleanDraws();

            foreach (TOrderable item in _draws)
                item.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            this.TryCleanUpdates();

            foreach (TOrderable item in _updates)
                item.TryUpdate(gameTime);
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

        #region Helper Methods
        protected virtual IEnumerable<TOrderable> RemapDraws()
        {
            return this.Where(o => o.Visible).OrderBy(o => o.DrawOrder);
        }

        protected virtual IEnumerable<TOrderable> RemapUpdates()
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
