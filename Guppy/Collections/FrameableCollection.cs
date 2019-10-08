using Guppy.Extensions.Collection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    public class FrameableCollection<TFrameable> : CreatableCollection<TFrameable>
        where TFrameable : Frameable
    {
        #region Private Attributes
        private IEnumerable<TFrameable> _draws;
        private IEnumerable<TFrameable> _updates;
        private ConcurrentQueue<TFrameable> _added;
        private ConcurrentQueue<TFrameable> _removed;
        private TFrameable _item;
        #endregion

        #region Protected Attributes
        protected Boolean dirtyDraws { get; set; }
        protected Boolean dirtyUpdates { get; set; }
        #endregion

        #region Public Attributes
        public IEnumerable<TFrameable> Draws { get { return _draws; } }
        public IEnumerable<TFrameable> Updates { get { return _updates; } }
        #endregion

        #region Constructor
        public FrameableCollection(IServiceProvider provider) : base(provider)
        {
            _added = new ConcurrentQueue<TFrameable>();
            _removed = new ConcurrentQueue<TFrameable>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;
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
                _draws = this.RemapDraws().ToArray();
                this.dirtyDraws = false;
            }
        }

        public virtual void TryCleanUpdates()
        {
            if (this.dirtyUpdates)
            {
                _updates = this.RemapUpdates();
                this.dirtyUpdates = false;
            }
        }
        #endregion

        #region Collection Methods
        public override Boolean Add(TFrameable item)
        {
            if (base.Add(item))
            {
                if (item.Visible)
                    this.dirtyDraws = true;
                if (item.Enabled)
                    this.dirtyUpdates = true;

                item.Events.TryAdd<Boolean>("visible:changed", this.HandleItemVisibleChanged);
                item.Events.TryAdd<Boolean>("enabled:changed", this.HandleItemEnabledChanged);

                return true;
            }

            return false;
        }

        public override Boolean Remove(TFrameable item)
        {
            if (base.Remove(item))
            {
                if (item.Visible)
                    this.dirtyDraws = true;
                if (item.Enabled)
                    this.dirtyUpdates = true;

                item.Events.TryRemove<Boolean>("visible:changed", this.HandleItemVisibleChanged);
                item.Events.TryRemove<Boolean>("enabled:changed", this.HandleItemEnabledChanged);

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        protected virtual IEnumerable<TFrameable> RemapDraws()
        {
            return this.Where(o => o.Visible).ToArray();
        }

        protected virtual IEnumerable<TFrameable> RemapUpdates()
        {
            return this.Where(o => o.Enabled).ToArray();
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
