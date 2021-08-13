using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;

namespace Guppy.Lists
{
    public class FrameableList<TFrameable> : FactoryServiceList<TFrameable>, IFrameable
        where TFrameable : class, IFrameable
    {
        #region Events
        public event Step OnPreDraw;
        public event Step OnDraw;
        public event Step OnPostDraw;
        public event Step OnPreUpdate;
        public event Step OnUpdate;
        public event Step OnPostUpdate;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnDraw += this.Draw;
            this.OnUpdate += this.Update;
        }

        protected override void Release()
        {
            base.Release();

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
            foreach (TFrameable child in this)
                child.TryDraw(gameTime);
        }

        protected virtual void Update(GameTime gameTime)
        {
            foreach (TFrameable child in this)
                child.TryUpdate(gameTime);
        }
        #endregion
    }
}
