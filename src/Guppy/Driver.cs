using Guppy.DependencyInjection;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Driver
    {
        #region Lifecycle Methods
        internal abstract void TryCreate(Object driven, ServiceProvider provider);
        internal abstract void TryDispose(Object driven);

        internal abstract void TryPreInitialize(Object driven, ServiceProvider provider);
        internal abstract void TryInitialize(Object driven, ServiceProvider provider);
        internal abstract void TryPostInitialize(Object driven, ServiceProvider provider);
        internal abstract void TryRelease(Object driven);
        #endregion
    }

    public class Driver<T> : Driver
        where T : Driven
    {
        protected T driven { get; private set; }

        #region Lifecycle Methods
        internal override void TryCreate(object driven, ServiceProvider provider)
        {
            ExceptionHelper.ValidateAssignableFrom<T>(driven.GetType());

            this.driven = driven as T;
            this.Create(this.driven, provider);
        }

        protected virtual void Create(T driven, ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        internal override void TryDispose(object driven)
            => this.Dispose(this.driven);

        protected virtual void Dispose(T driven)
        {
            // throw new NotImplementedException();
        }

        internal override void TryInitialize(object driven, ServiceProvider provider)
            => this.Initialize(this.driven, provider);

        protected virtual void Initialize(T driven, ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        internal override void TryPostInitialize(object driven, ServiceProvider provider)
            => this.PostInitialize(this.driven, provider);

        protected virtual void PostInitialize(T driven, ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        internal override void TryPreInitialize(object driven, ServiceProvider provider)
            => this.PreInitialize(this.driven, provider);

        protected virtual void PreInitialize(T driven, ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        internal override void TryRelease(object driven)
            => this.Release(this.driven);

        protected virtual void Release(T driven)
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
