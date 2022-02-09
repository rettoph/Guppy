using Guppy.EntityComponent.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.EntityComponent.Lists
{
    public class OrderableList<TOrderable> : FrameableList<TOrderable>
        where TOrderable : class, IOrderable
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnAdd += this.AddItem;
            this.OnRemove += this.RemoveItem;
        }

        protected override void Uninitialize()
        {
            base.Uninitialize();

            this.OnAdd -= this.AddItem;
            this.OnRemove -= this.RemoveItem;
        }
        #endregion

        #region Collection Methods
        private void AddItem(TOrderable item)
        {
            item.OnUpdateOrderChanged += this.HandleItemUpdateOrderChanged;
            item.OnDrawOrderChanged += this.HandleItemDrawOrderChanged;
        }

        private void RemoveItem(TOrderable item)
        {
            item.OnUpdateOrderChanged -= this.HandleItemUpdateOrderChanged;
            item.OnDrawOrderChanged -= this.HandleItemDrawOrderChanged;
        }
        #endregion

        #region Helper Methods
        protected override IEnumerable<TOrderable> RemapDraws()
        {
            return base.RemapDraws().OrderBy(o => o.DrawOrder);
        }

        protected override IEnumerable<TOrderable> RemapUpdates()
        {
            return base.RemapUpdates().OrderBy(o => o.UpdateOrder);
        }
        #endregion

        #region Event Handlers
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
