using Guppy.Extensions.Collection;
using Guppy.Interfaces;
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
        where TFrameable : IFrameable
    {
        #region Private Attributes
        private List<TFrameable> _draws;
        private List<TFrameable> _updates;
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

            _draws = new List<TFrameable>();
            _updates = new List<TFrameable>();

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

        #region Collection Methods
        public override Boolean Add(TFrameable item)
        {
            if (base.Add(item))
            {
                if (item.Visible)
                    this.dirtyDraws = true;
                if (item.Enabled)
                    this.dirtyUpdates = true;

                item.OnVisibleChanged += this.HandleItemVisibleChanged;
                item.OnEnabledChanged += this.HandleItemEnabledChanged;

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

                item.OnVisibleChanged -= this.HandleItemVisibleChanged;
                item.OnEnabledChanged -= this.HandleItemEnabledChanged;

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        protected virtual IEnumerable<TFrameable> RemapDraws()
        {
            return this.Where(o => o.Visible);
        }

        protected virtual IEnumerable<TFrameable> RemapUpdates()
        {
            return this.Where(o => o.Enabled);
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
