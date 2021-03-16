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
        internal abstract void TryInitialize(Driven driven, ServiceProvider provider);
        internal abstract void TryRelease(Driven driven);
        #endregion
    }

    public class Driver<T> : Driver
        where T : Driven
    {
        protected T driven { get; private set; }

        #region Lifecycle Methods
        internal override void TryInitialize(Driven driven, ServiceProvider provider)
        {
            ExceptionHelper.ValidateAssignableFrom<T>(driven.GetType());

            this.driven = driven as T;
            this.Initialize(this.driven, provider);
        }
        

        protected virtual void Initialize(T driven, ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        internal override void TryRelease(Driven driven)
        {
            this.Release(this.driven);
            this.driven = null;
        }

        protected virtual void Release(T driven)
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
