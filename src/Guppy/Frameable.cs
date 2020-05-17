using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Frameable : Service, IFrameable
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
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnPreDraw += this.PreDraw;
            this.OnDraw += this.Draw;
            this.OnPostDraw += this.PostDraw;

            this.OnPreUpdate += this.PreUpdate;
            this.OnUpdate += this.Update;
            this.OnPostUpdate += this.PostUpdate;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnPreDraw -= this.PreDraw;
            this.OnDraw -= this.Draw;
            this.OnPostDraw -= this.PostDraw;

            this.OnPreUpdate -= this.PreUpdate;
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

        protected virtual void PreDraw(GameTime gameTime)
        {
            //
        }
        protected virtual void Draw(GameTime gameTime)
        {
            // 
        }
        protected virtual void PostDraw(GameTime gameTime)
        {
            //
        }

        protected virtual void PreUpdate(GameTime gameTime)
        {
            //
        }
        protected virtual void Update(GameTime gameTime)
        {
            // 
        }
        protected virtual void PostUpdate(GameTime gameTime)
        {
            //
        }
        #endregion
    }
}
