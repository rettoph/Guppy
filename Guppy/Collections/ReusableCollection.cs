using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Contains a collecion of reusable objects and automatically
    /// removes the objects when they are disposed.
    /// </summary>
    /// <typeparam name="TResusable"></typeparam>
    public class ReusableCollection<TResusable> : FrameableCollection<TResusable>, IDisposable
        where TResusable : class, IReusable
    {
        #region Constructors
        public ReusableCollection(IServiceProvider provider) : base(provider)
        {
        }
        #endregion

        #region Lifecycle Methods
        public void Dispose()
        {
            // auto dispose children
            while(this.Count > 0)
                this.First().Dispose();
        }
        #endregion

        #region Frame Methods

        #endregion

        #region Collection Methods
        public override bool Add(TResusable item)
        {
            if(base.Add(item))
            {
                // Bind to any relevant events
                item.Events.AddDelegate<Boolean>("changed:enabled", this.HandleItemEnabledChanged);
                item.Events.AddDelegate<Boolean>("changed:visible", this.HandleItemVisibleChanged);
                item.Events.AddDelegate<Int32>("changed:update-order", this.HandleItemUpdateOrderChanged);
                item.Events.AddDelegate<Int32>("changed:draw-order", this.HandleItemDrawOrderChanged);
                item.Events.AddDelegate<DateTime>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        public override Boolean Remove(TResusable item)
        {
            if(base.Remove(item))
            {
                // Unbind all related events
                item.Events.RemoveDelegate<Boolean>("changed:enabled", this.HandleItemEnabledChanged);
                item.Events.RemoveDelegate<Boolean>("changed:visible", this.HandleItemVisibleChanged);
                item.Events.RemoveDelegate<Int32>("changed:update-order", this.HandleItemUpdateOrderChanged);
                item.Events.RemoveDelegate<Int32>("changed:draw-order", this.HandleItemDrawOrderChanged);
                item.Events.RemoveDelegate<DateTime>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
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

        private void HandleItemDisposing(object sender, DateTime arg)
        {
            // Auto remove the child on dispose
            this.Remove(sender as TResusable);
        }
        #endregion
    }
}
