using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Guppy.Collections
{
    public class FrameableCollection<TFrameable> : UniqueCollection<TFrameable>
        where TFrameable : class, IFrameable
    {
        #region Private Attributes
        private IEnumerable<TFrameable> _draws;
        private IEnumerable<TFrameable> _updates;
        #endregion

        #region Protected Attributes
        protected Boolean dirtyDraws { get; set; }
        protected Boolean dirtyUpdates { get; set; }
        #endregion

        #region Constructors
        public FrameableCollection(IServiceProvider provider) : base(provider)
        {
            this.RemapDraws();
            this.RemapUpdates();
        }
        #endregion

        #region Frame Methods
        public virtual void TryUpdate(GameTime gameTime)
        {
            if(this.dirtyUpdates)
            {
                this.RemapUpdates();
                this.dirtyUpdates = false;
            }

            foreach (TFrameable frameable in _updates)
                frameable.TryUpdate(gameTime);
        }

        public virtual void TryDraw(GameTime gameTime)
        {
            if (this.dirtyDraws)
            {
                this.RemapDraws();
                this.dirtyDraws = false;
            }

            foreach (TFrameable frameable in _updates)
                frameable.TryDraw(gameTime);
        }
        #endregion

        #region Collection Methods
        public virtual new Boolean Add(TFrameable item)
        {
            if(base.Add(item))
            {
                // Mark draws and updates as dirty at this time
                this.dirtyDraws = true;
                this.dirtyUpdates = true;

                // Bind to any relevant events
                item.Events.AddDelegate<Boolean>("changed:enabled", this.HandleItemEnabledChanged);
                item.Events.AddDelegate<Boolean>("changed:visible", this.HandleItemVisibleChanged);
                item.Events.AddDelegate<Int32>("changed:update-order", this.HandleItemUpdateOrderChanged);
                item.Events.AddDelegate<Int32>("changed:draw-order", this.HandleItemDrawOrderChanged);

                return true;
            }

            return false;
        }

        public virtual new Boolean Remove(TFrameable item)
        {
            if (base.Remove(item))
            {
                // Mark draws and updates as dirty at this time
                this.dirtyDraws = true;
                this.dirtyUpdates = true;

                // Unbind all related events
                item.Events.RemoveDelegate<Boolean>("changed:enabled", this.HandleItemEnabledChanged);
                item.Events.RemoveDelegate<Boolean>("changed:visible", this.HandleItemVisibleChanged);
                item.Events.RemoveDelegate<Int32>("changed:update-order", this.HandleItemUpdateOrderChanged);
                item.Events.RemoveDelegate<Int32>("changed:draw-order", this.HandleItemDrawOrderChanged);

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
