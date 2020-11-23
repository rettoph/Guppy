using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Lists
{
    public class FrameableList<TFrameable> : ServiceList<TFrameable>, IFrameable
        where TFrameable : Frameable
    {
        #region Private Fields
        private Queue<TFrameable> _released;
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
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _released = new Queue<TFrameable>();

            this.OnDraw += this.Draw;
            this.OnUpdate += this.Update;
            this.OnPostUpdate += this.PostUpdate;
        }

        protected override void Release()
        {
            base.Release();

            this.OnDraw -= this.Draw;
            this.OnUpdate -= this.Update;
            this.OnPostUpdate -= this.PostUpdate;
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
            this.ForEach(u => u.TryDraw(gameTime));
        }

        protected virtual void Update(GameTime gameTime)
        {
            this.ForEach(u => u.TryUpdate(gameTime));
        }

        private void PostUpdate(GameTime gameTime)
        {
            while (_released.Any())
                base.HandleItemReleased(_released.Dequeue());
        }
        #endregion

        #region Event Handlers
        protected override void HandleItemReleased(IService sender)
            => _released.Enqueue(sender as TFrameable);
        #endregion
    }
}
