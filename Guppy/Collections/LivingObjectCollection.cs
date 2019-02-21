using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;

namespace Guppy.Collections
{
    public class LivingObjectCollection<TLivingObject>
        where TLivingObject : class, ILivingObject
    {
        #region Private Fields
        private List<TLivingObject> _list;
        private IOrderedEnumerable<TLivingObject> _drawables;
        private IOrderedEnumerable<TLivingObject> _updatables;
        private Boolean _dirtyUpdatables;
        private Boolean _dirtyDrawables;
        #endregion

        #region Public Attributes
        public Int32 Count { get { return _list.Count; } }
        public TLivingObject this[int index] { get { return _list[index]; } set { _list[index] = value; } }
        #endregion

        #region Constructors
        public LivingObjectCollection()
        {
            _list = new List<TLivingObject>();
        }
        #endregion

        #region Frame Methods
        public void Draw(GameTime gameTime)
        {
            if(_dirtyDrawables)
            { // Update the drawables array
                _drawables = _list.Where(lo => lo.Visible)
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
                _updatables = _list.Where(lo => lo.Enabled)
                    .OrderBy(lo => lo.UpdateOrder);

                _dirtyUpdatables = false;
            }

            // Update all the updatables
            foreach (ILivingObject livingObject in _updatables)
                livingObject.Update(gameTime);
        }
        #endregion

        #region Collection Methods
        public virtual void Add(TLivingObject item)
        {
            _list.Add(item);

            item.UpdateOrderChanged += this.HandleUpdateOrderChanged;
            item.EnabledChanged     += this.HandleEnabledChanged;

            item.DrawOrderChanged += this.HandleDrawOrderChanged;
            item.VisibleChanged   += this.HandleVisibleChanged;

            item.Disposing += this.HandleDisposing;
        }

        public virtual Boolean Remove(TLivingObject item)
        {
            _list.Remove(item);

            item.UpdateOrderChanged -= this.HandleUpdateOrderChanged;
            item.EnabledChanged     -= this.HandleEnabledChanged;

            item.DrawOrderChanged -= this.HandleDrawOrderChanged;
            item.VisibleChanged   -= this.HandleVisibleChanged;

            item.Disposing -= this.HandleDisposing;

            return true;
        }

        public virtual void Clear()
        {
            while (_list.Count > 0)
                this.Remove(_list[0]);
        }

        public virtual Boolean Contains(TLivingObject item)
        {
            return _list.Contains(item);
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

        private void HandleDisposing(object sender, ILivingObject item)
        {
            this.Remove(item as TLivingObject);
        }
        #endregion
    }
}
