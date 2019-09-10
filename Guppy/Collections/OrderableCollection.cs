using Guppy.Extensions.Collection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Can contain a collection of frameable
    /// objects. This will automatically maintain
    /// an ordered list of items organized by draw order
    /// and updated order for easy drawing.
    /// </summary>
    /// <typeparam name="TOrderable"></typeparam>
    public class OrderableCollection<TOrderable> : FrameableCollection<TOrderable>
        where TOrderable : Orderable
    {
        #region Constructors
        public OrderableCollection(IServiceProvider provider) : base(provider)
        {
        }
        #endregion

        #region Collection Methods
        public override Boolean Add(TOrderable item)
        {
            if (base.Add(item))
            {
                item.Events.TryAdd<Int32>("update-order:changed", this.HandleItemUpdateOrderChanged);
                item.Events.TryAdd<Int32>("draw-order:changed", this.HandleItemDrawOrderChanged);

                return true;
            }

            return false;
        }

        public override Boolean Remove(TOrderable item)
        {
            if (base.Remove(item))
            {
                item.Events.TryRemove<Int32>("update-order:changed", this.HandleItemUpdateOrderChanged);
                item.Events.TryRemove<Int32>("draw-order:changed", this.HandleItemDrawOrderChanged);

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        protected override IEnumerable<TOrderable> RemapDraws()
        {
            return base.RemapUpdates().OrderBy(o => o.DrawOrder);
        }

        protected override IEnumerable<TOrderable> RemapUpdates()
        {
            return base.RemapUpdates().OrderBy(o => o.UpdateOrder);
        }
        #endregion

        #region Event Handlers
        private void HandleItemDrawOrderChanged(object sender, int arg)
        {
            this.dirtyDraws = true;
        }

        private void HandleItemUpdateOrderChanged(object sender, int arg)
        {
            this.dirtyUpdates = true;
        }
        #endregion
    }
}
