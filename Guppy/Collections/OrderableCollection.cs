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
    public class OrderableCollection<TOrderable> : CreatableCollection<TOrderable>
        where TOrderable : Orderable
    {
        #region Private Attributes
        private IEnumerable<TOrderable> _draws;
        private IEnumerable<TOrderable> _updates;
        #endregion

        #region Protected Attributes
        protected Boolean dirtyDraws { get; set; }
        protected Boolean dirtyUpdates { get; set; }
        #endregion

        #region Public Attributes
        public IEnumerable<TOrderable> Draws { get { return _draws; } }
        public IEnumerable<TOrderable> Updates { get { return _updates; } }
        #endregion

        #region Constructors
        public OrderableCollection(IServiceProvider provider) : base(provider)
        {
            this.RemapDraws();
            this.RemapUpdates();
        }
        #endregion

        #region Frame Methods
        public virtual void TryUpdate(GameTime gameTime)
        {
            this.TryCleanUpdates();

            _updates.TryUpdateAll(gameTime);
        }

        public virtual void TryDraw(GameTime gameTime)
        {
            this.TryCleanDraws();

            _draws.TryDrawAll(gameTime);
        }

        public virtual void TryCleanDraws()
        {
            if (this.dirtyDraws)
            {
                this.RemapDraws();
                this.dirtyDraws = false;
            }
        }

        public virtual void TryCleanUpdates()
        {
            if (this.dirtyUpdates)
            {
                this.RemapUpdates();
                this.dirtyUpdates = false;
            }
        }
        #endregion

        #region Collection Methods
        public virtual new Boolean Add(TOrderable item)
        {
            if (base.Add(item))
            {
                // Mark draws and updates as dirty at this time
                this.dirtyDraws = true;
                this.dirtyUpdates = true;

                // Bind to any relevant events
                item.Events.TryAdd<Boolean>("changed:enabled", this.HandleItemEnabledChanged);
                item.Events.TryAdd<Boolean>("changed:visible", this.HandleItemVisibleChanged);
                item.Events.TryAdd<Int32>("changed:update-order", this.HandleItemUpdateOrderChanged);
                item.Events.TryAdd<Int32>("changed:draw-order", this.HandleItemDrawOrderChanged);

                return true;
            }

            return false;
        }

        public virtual new Boolean Remove(TOrderable item)
        {
            if (base.Remove(item))
            {
                // Mark draws and updates as dirty at this time
                this.dirtyDraws = true;
                this.dirtyUpdates = true;

                // Unbind all related events
                item.Events.TryRemove<Boolean>("changed:enabled", this.HandleItemEnabledChanged);
                item.Events.TryRemove<Boolean>("changed:visible", this.HandleItemVisibleChanged);
                item.Events.TryRemove<Int32>("changed:update-order", this.HandleItemUpdateOrderChanged);
                item.Events.TryRemove<Int32>("changed:draw-order", this.HandleItemDrawOrderChanged);

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        protected void RemapDraws()
        {
            _draws = this
                .OrderBy(f => f.DrawOrder);
        }

        protected void RemapUpdates()
        {
            _updates = this
                .OrderBy(f => f.UpdateOrder);
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

        private void HandleItemVisibleChanged(object sender, bool arg)
        {
            this.dirtyDraws = true;
        }

        private void HandleItemEnabledChanged(object sender, bool arg)
        {
            this.dirtyUpdates = true;
        }
        #endregion
    }
}
