using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Simple collection used to contain several IOrderable insatnces.
    /// 
    /// This will automatically manage 2 lists of Visible/Enabled items
    /// and each frame will update both lists in Update/Draw order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderableCollection<T> : FactoryCollection<T>, IFrameable
        where T : Orderable
    {
        #region Private Attributes
        private List<T> _draws;
        private List<T> _updates;
        #endregion

        #region Events
        public event Step OnPreDraw;
        public event Step OnDraw;
        public event Step OnPostDraw;
        public event Step OnPreUpdate;
        public event Step OnUpdate;
        public event Step OnPostUpdate;
        #endregion

        #region Protected Attributes
        protected Boolean dirtyDraws { get; set; }
        protected Boolean dirtyUpdates { get; set; }
        #endregion

        #region Public Attributes
        public IEnumerable<T> Draws { get { return _draws; } }
        public IEnumerable<T> Updates { get { return _updates; } }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnDraw += this.Draw;
            this.OnUpdate += this.Update;

            this.OnAdd += this.AddItem;
            this.OnRemove += this.RemoveItem;
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _draws = new List<T>();
            _updates = new List<T>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;
        }

        protected override void Dispose()
        {
            base.Dispose();

            _draws.Clear();
            _updates.Clear();

            this.OnDraw -= this.Draw;
            this.OnUpdate -= this.Update;

            this.OnAdd -= this.AddItem;
            this.OnRemove -= this.RemoveItem;
        }
        #endregion

        #region Collection Methods
        private void AddItem(T item)
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

        private void RemoveItem(T item)
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
        public virtual void TryDraw(GameTime gameTime)
        {
            this.OnPreDraw?.Invoke(gameTime);
            this.OnDraw?.Invoke(gameTime);
            this.OnPostDraw?.Invoke(gameTime);
        }

        public virtual void TryUpdate(GameTime gameTime)
        {
            this.OnPreUpdate?.Invoke(gameTime);
            this.OnUpdate?.Invoke(gameTime);
            this.OnPostUpdate?.Invoke(gameTime);
        }

        protected virtual void Draw(GameTime gameTime)
        {
            this.TryCleanDraws();

            _draws.ForEach(u => u.TryDraw(gameTime));
        }

        protected virtual void Update(GameTime gameTime)
        {
            this.TryCleanUpdates();

            _updates.ForEach(u => u.TryUpdate(gameTime));
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
        protected virtual IEnumerable<T> RemapDraws()
        {
            return this.Where(o => o.Visible).OrderBy(o => o.DrawOrder);
        }

        protected virtual IEnumerable<T> RemapUpdates()
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
