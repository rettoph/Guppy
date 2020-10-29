using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Lists
{
    public class OrderableList<TOrderable> : ServiceList<TOrderable>, IFrameable
        where TOrderable : Orderable
    {
        #region Private Attributes
        private List<TOrderable> _draws;
        private List<TOrderable> _updates;
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
        public IEnumerable<TOrderable> Draws { get { return _draws; } }
        public IEnumerable<TOrderable> Updates { get { return _updates; } }
        #endregion

        #region COnstructors
        public OrderableList(Boolean autofill = false) : base(autofill)
        {

        }
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

            _draws = new List<TOrderable>();
            _updates = new List<TOrderable>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;
        }

        protected override void Release()
        {
            base.Release();

            _draws.Clear();
            _updates.Clear();

            this.OnDraw -= this.Draw;
            this.OnUpdate -= this.Update;

            this.OnAdd -= this.AddItem;
            this.OnRemove -= this.RemoveItem;
        }
        #endregion

        #region Collection Methods
        private void AddItem(TOrderable item)
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

        private void RemoveItem(TOrderable item)
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
        protected virtual IEnumerable<TOrderable> RemapDraws()
        {
            return this.Where(o => o.Visible).OrderBy(o => o.DrawOrder);
        }

        protected virtual IEnumerable<TOrderable> RemapUpdates()
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
