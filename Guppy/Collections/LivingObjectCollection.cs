using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;

namespace Guppy.Collections
{
    public class LivingObjectCollection<TLivingObject> : TrackedDisposableCollection<TLivingObject>
        where TLivingObject : class, ILivingObject
    {
        #region Private Fields
        private IOrderedEnumerable<TLivingObject> _drawables;
        private IOrderedEnumerable<TLivingObject> _updatables;
        private Boolean _dirtyUpdatables;
        private Boolean _dirtyDrawables;
        #endregion

        #region Constructors
        public LivingObjectCollection()
        {
        }
        #endregion

        #region Frame Methods
        public void Draw(GameTime gameTime)
        {
            if(_dirtyDrawables)
            { // Update the drawables array
                _drawables = this.list.Where(lo => lo.Visible)
                    .OrderBy(lo => lo.DrawOrder);

                _dirtyDrawables = false;
            }

            // Update all the drawables
            foreach (ILivingObject livingObject in _drawables)
                livingObject.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (_dirtyUpdatables)
            { // Update the updatables array
                _updatables = this.list.Where(lo => lo.Enabled)
                    .OrderBy(lo => lo.UpdateOrder);

                _dirtyUpdatables = false;
            }

            // Update all the updatables
            foreach (ILivingObject livingObject in _updatables)
                livingObject.Update(gameTime);
        }
        #endregion

        #region Collection Methods
        public override void Add(TLivingObject item)
        {
            base.Add(item);

            item.UpdateOrderChanged += this.HandleUpdateOrderChanged;
            item.EnabledChanged     += this.HandleEnabledChanged;

            item.DrawOrderChanged += this.HandleDrawOrderChanged;
            item.VisibleChanged   += this.HandleVisibleChanged;
        }

        public override Boolean Remove(TLivingObject item)
        {
            if(base.Remove(item))
            {
                item.UpdateOrderChanged -= this.HandleUpdateOrderChanged;
                item.EnabledChanged -= this.HandleEnabledChanged;

                item.DrawOrderChanged -= this.HandleDrawOrderChanged;
                item.VisibleChanged -= this.HandleVisibleChanged;

                return true;
            }

            return false;
        }
        #endregion

        #region Event Handlers
        private void HandleVisibleChanged(object sender, EventArgs e)
        {
            _dirtyDrawables = true;
        }

        private void HandleDrawOrderChanged(object sender, EventArgs e)
        {
            _dirtyDrawables = true;
        }

        private void HandleEnabledChanged(object sender, EventArgs e)
        {
            _dirtyUpdatables = true;
        }

        private void HandleUpdateOrderChanged(object sender, EventArgs e)
        {
            _dirtyUpdatables = true;
        }
        #endregion
    }
}
