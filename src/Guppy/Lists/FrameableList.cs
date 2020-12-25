﻿using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;

namespace Guppy.Lists
{
    public class FrameableList<TFrameable> : ServiceList<TFrameable>, IFrameable
        where TFrameable : Frameable
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
            this.OnPreDraw.LogInvocationList($"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id}).OnPreDraw", 1);
            this.OnDraw.LogInvocationList($"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id}).OnDraw", 1);
            this.OnPostDraw.LogInvocationList($"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id}).OnPostDraw", 1);
            this.OnPreUpdate.LogInvocationList($"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id}).OnPreUpdate", 1);
            this.OnUpdate.LogInvocationList($"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id}).OnUpdate", 1);
            this.OnPostUpdate.LogInvocationList($"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id}).OnPostUpdate", 1);
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
