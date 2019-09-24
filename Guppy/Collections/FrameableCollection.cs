using Guppy.Extensions.Collection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
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
        private Queue<TFrameable> _added;
        private Queue<TFrameable> _removed;
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
            _added = new Queue<TFrameable>();
            _removed = new Queue<TFrameable>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;
        }
        #endregion

        #region Frame Methods
        public virtual void TryUpdate(GameTime gameTime)
        {
            this.Flush();

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
            if (!this.Contains(item))
            {
                _added.Enqueue(item);

                return true;
            }

            return false;
        }

        public override Boolean Remove(TFrameable item)
        {
            if (this.Contains(item))
            {
                _removed.Enqueue(item);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Flush the enqueued added & removed items
        /// </summary>
        public void Flush()
        {
            while (_added.Any())
                this.FlushAdd(_added.Dequeue());

            while (_removed.Any())
                this.FlushRemove(_removed.Dequeue());
        }

        private void FlushAdd(TFrameable item)
        {
            if (base.Add(item))
            {
                // Mark draws and updates as dirty at this time
                this.dirtyDraws = true;
                this.dirtyUpdates = true;

                // Bind to any relevant events
                item.Events.TryAdd<Boolean>("enabled:changed", this.HandleItemEnabledChanged);
                item.Events.TryAdd<Boolean>("visible:changed", this.HandleItemVisibleChanged);
            }
        }

        private void FlushRemove(TFrameable item)
        {
            if (base.Remove(item))
            {
                // Mark draws and updates as dirty at this time
                this.dirtyDraws = true;
                this.dirtyUpdates = true;

                // Unbind all related events
                item.Events.TryRemove<Boolean>("enabled:changed", this.HandleItemEnabledChanged);
                item.Events.TryRemove<Boolean>("visible:changed", this.HandleItemVisibleChanged);
            }
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

        public override TFrameable GetById(Guid id)
        {
            this.Flush();

            return base.GetById(id);
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
