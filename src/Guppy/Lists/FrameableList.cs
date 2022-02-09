using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Guppy.EntityComponent.DependencyInjection;
using System.Linq;

namespace Guppy.EntityComponent.Lists
{
    public class FrameableList<TFrameable> : FactoryServiceList<TFrameable>
        where TFrameable : class, IFrameable
    {
        #region Private Attributes
        private List<TFrameable> _draws;
        private List<TFrameable> _updates;

        private ConcurrentDictionary<Guid, TFrameable> _dirtySource;
        #endregion

        #region Public Attributes
        public IEnumerable<TFrameable> Draws { get { return _draws; } }
        public IEnumerable<TFrameable> Updates { get { return _updates; } }
        #endregion

        #region Protected Attributes
        protected Boolean dirtyDraws { get; set; }
        protected Boolean dirtyUpdates { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _draws = new List<TFrameable>();
            _updates = new List<TFrameable>();

            _dirtySource = new ConcurrentDictionary<Guid, TFrameable>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;

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

        #region Frame Methods
        public virtual void TryDraw(GameTime gameTime)
        {
            this.Draw(gameTime);
        }

        public virtual void TryUpdate(GameTime gameTime)
        {
            this.Update(gameTime);
        }

        protected virtual void Draw(GameTime gameTime)
        {
            this.TryCleanDraws();

            foreach (TFrameable child in _draws)
                child.TryDraw(gameTime);
        }

        protected virtual void Update(GameTime gameTime)
        {
            this.TryCleanUpdates();

            foreach (TFrameable child in _updates)
                child.TryUpdate(gameTime);
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
        protected virtual IEnumerable<TFrameable> RemapDraws()
        {
            return _dirtySource.Values.Where(o => o.Visible);
        }

        protected virtual IEnumerable<TFrameable> RemapUpdates()
        {
            return _dirtySource.Values.Where(o => o.Enabled);
        }
        #endregion

        #region Collection Methods
        private void AddItem(TFrameable item)
        {
            _dirtySource.TryAdd(item.Id, item);

            item.OnVisibleChanged += this.HandleItemVisibleChanged;
            item.OnEnabledChanged += this.HandleItemEnabledChanged;

            if (this.IsDirtyDraw(item))
                this.dirtyDraws = true;
            if (this.IsDirtyUpdate(item))
                this.dirtyUpdates = true;
        }

        private void RemoveItem(TFrameable item)
        {
            _dirtySource.TryRemove(item.Id, out _);

            item.OnVisibleChanged -= this.HandleItemVisibleChanged;
            item.OnEnabledChanged -= this.HandleItemEnabledChanged;

            if (this.IsDirtyDraw(item))
                this.dirtyDraws = true;
            if (this.IsDirtyUpdate(item))
                this.dirtyUpdates = true;
        }

        protected virtual Boolean IsDirtyDraw(TFrameable item)
        {
            return item.Visible;
        }

        protected virtual Boolean IsDirtyUpdate(TFrameable item)
        {
            return item.Enabled;
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
        #endregion
    }
}
