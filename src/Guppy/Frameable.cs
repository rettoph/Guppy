﻿using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace Guppy
{
    public abstract class Frameable : Entity, IFrameable
    {
        #region Events
        /// <inheritdoc/>
        public event Step OnPreDraw;
        /// <inheritdoc/>
        public event Step OnDraw;
        /// <inheritdoc/>
        public event Step OnPostDraw;
        /// <inheritdoc/>
        public event Step OnPreUpdate;
        /// <inheritdoc/>
        public event Step OnUpdate;
        /// <inheritdoc/>
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

        protected override void PostRelease()
        {
            base.PostRelease();

#if DEBUG_VERBOSE
            this.OnPreDraw.LogInvocationList($"OnPreDraw", this);
            this.OnDraw.LogInvocationList("OnDraw", this);
            this.OnPostDraw.LogInvocationList("OnPostDraw", this);
            this.OnPreUpdate.LogInvocationList("OnPreUpdate", this);
            this.OnUpdate.LogInvocationList("OnUpdate", this);
            this.OnPostUpdate.LogInvocationList("OnPostUpdate", this);
#endif
        }

        protected override void Release()
        {
            base.Release();

            this.OnPreDraw -= this.PreDraw;
            this.OnDraw -= this.Draw;
            this.OnPostDraw -= this.PostDraw;

            this.OnPreUpdate -= this.PreUpdate;
            this.OnUpdate -= this.Update;
            this.OnPostUpdate -= this.PostUpdate;
        }
        #endregion

        #region Frame Methods
        /// <inheritdoc/>
        public virtual void TryDraw(GameTime gameTime)
        {
            this.OnPreDraw?.Invoke(gameTime);
            this.OnDraw?.Invoke(gameTime);
            this.OnPostDraw?.Invoke(gameTime);
        }

        /// <inheritdoc/>
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
