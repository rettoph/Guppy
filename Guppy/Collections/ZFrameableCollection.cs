using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;

namespace Guppy.Collections
{
    public class ZFrameableCollection<TLivingObject> : InitializableCollection<TLivingObject>
        where TLivingObject : class, IZFrameable
    {
        #region Private Fields
        private IOrderedEnumerable<TLivingObject> _drawables;
        private IOrderedEnumerable<TLivingObject> _updatables;
        private Boolean _dirtyUpdatables;
        private Boolean _dirtyDrawables;
        #endregion

        #region Constructors
        public ZFrameableCollection(bool disposeOnRemove = true, bool initializeOnAdd = true) : base(disposeOnRemove, initializeOnAdd)
        {
            _dirtyDrawables = true;
            _dirtyUpdatables = true;
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
            foreach (IZFrameable livingObject in _drawables)
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
            foreach (IZFrameable livingObject in _updatables)
                livingObject.Update(gameTime);
        }
        #endregion

        #region Collection Methods
        public override void Add(TLivingObject item)
        {
            base.Add(item);

            item.Events.AddHandler("set:update-order", this.HandleUpdateOrderChanged);
            item.Events.AddHandler("set:enabled", this.HandleEnabledChanged);

            item.Events.AddHandler("set:draw-order", this.HandleDrawOrderChanged);
            item.Events.AddHandler("set:visible", this.HandleVisibleChanged);
        }

        public override Boolean Remove(TLivingObject item)
        {
            if(base.Remove(item))
            {
                item.Events.RemoveHandler("set:update-order", this.HandleUpdateOrderChanged);
                item.Events.RemoveHandler("set:enabled", this.HandleEnabledChanged);

                item.Events.RemoveHandler("set:draw-order", this.HandleDrawOrderChanged);
                item.Events.RemoveHandler("set:visible", this.HandleVisibleChanged);

                return true;
            }

            return false;
        }
        #endregion

        #region Event Handlers
        private void HandleVisibleChanged(Object e)
        {
            _dirtyDrawables = true;
        }

        private void HandleDrawOrderChanged(Object e)
        {
            _dirtyDrawables = true;
        }

        private void HandleEnabledChanged(Object e)
        {
            _dirtyUpdatables = true;
        }

        private void HandleUpdateOrderChanged(Object e)
        {
            _dirtyUpdatables = true;
        }
        #endregion
    }
}
