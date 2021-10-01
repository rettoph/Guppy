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
        }

        protected override void Release()
        {
            base.Release();

            this.OnAdd -= this.AddItem;
            this.OnRemove -= this.RemoveItem;
        }
        #endregion

        #region Collection Methods
        private void AddItem(TOrderable item)
        {
            item.OnVisibleChanged += this.HandleItemVisibleChanged;
            item.OnEnabledChanged += this.HandleItemEnabledChanged;
            item.OnUpdateOrderChanged += this.HandleItemUpdateOrderChanged;
            item.OnDrawOrderChanged += this.HandleItemDrawOrderChanged;
        }

        private void RemoveItem(TOrderable item)
        {
            item.OnVisibleChanged -= this.HandleItemVisibleChanged;
            item.OnEnabledChanged -= this.HandleItemEnabledChanged;
            item.OnUpdateOrderChanged -= this.HandleItemUpdateOrderChanged;
            item.OnDrawOrderChanged -= this.HandleItemDrawOrderChanged;
        }

        protected override bool IsDirtyDraw(TOrderable item)
        {
            return item.Visible;
        }

        protected override bool IsDirtyUpdate(TOrderable item)
        {
            return item.Enabled;
        }
        #endregion

        #region Helper Methods
        protected override IEnumerable<TOrderable> RemapDraws()
        {
            return base.RemapDraws().Where(o => o.Visible).OrderBy(o => o.DrawOrder);
        }

        protected override IEnumerable<TOrderable> RemapUpdates()
        {
            return base.RemapUpdates().Where(o => o.Enabled).OrderBy(o => o.UpdateOrder);
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
