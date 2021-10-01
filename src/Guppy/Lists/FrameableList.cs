using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Guppy.Lists
{
    public class FrameableList<TFrameable> : FactoryServiceList<TFrameable>, IFrameable
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

        #region Events
        public event Step OnPreDraw;
        public event Step OnDraw;
        public event Step OnPostDraw;
        public event Step OnPreUpdate;
        public event Step OnUpdate;
        public event Step OnPostUpdate;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            _draws = new List<TFrameable>();
            _updates = new List<TFrameable>();

            _dirtySource = new ConcurrentDictionary<Guid, TFrameable>();

            this.dirtyDraws = true;
            this.dirtyUpdates = true;

            this.OnDraw += this.Draw;
            this.OnUpdate += this.Update;

            this.OnAdd += this.AddItem;
            this.OnRemove += this.RemoveItem;
        }

        protected override void Release()
        {
            base.Release();

            _draws.Clear();
            _updates.Clear();


            this.OnAdd -= this.AddItem;
            this.OnRemove -= this.RemoveItem;

            this.OnDraw -= this.Draw;
            this.OnUpdate -= this.Update;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

#if DEBUG_VERBOSE
            this.OnPreDraw.LogInvocationList("OnPreDraw", this, 1);
            this.OnDraw.LogInvocationList("OnDraw", this, 1);
            this.OnPostDraw.LogInvocationList("OnPostDraw", this, 1);
            this.OnPreUpdate.LogInvocationList("OnPreUpdate", this, 1);
            this.OnUpdate.LogInvocationList("OnUpdate", this, 1);
            this.OnPostUpdate.LogInvocationList("OnPostUpdate", this, 1);
#endif
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
            return _dirtySource.Values;
        }

        protected virtual IEnumerable<TFrameable> RemapUpdates()
        {
            return _dirtySource.Values;
        }
        #endregion

        #region Collection Methods
        private void AddItem(TFrameable item)
        {
            _dirtySource.TryAdd(item.Id, item);

            if (this.IsDirtyDraw(item))
                this.dirtyDraws = true;
            if (this.IsDirtyUpdate(item))
                this.dirtyUpdates = true;
        }

        private void RemoveItem(TFrameable item)
        {
            _dirtySource.TryRemove(item.Id, out _);

            if (this.IsDirtyDraw(item))
                this.dirtyDraws = true;
            if (this.IsDirtyUpdate(item))
                this.dirtyUpdates = true;
        }

        protected virtual Boolean IsDirtyDraw(TFrameable item)
        {
            return true;
        }

        protected virtual Boolean IsDirtyUpdate(TFrameable item)
        {
            return true;
        }
        #endregion
    }
}
