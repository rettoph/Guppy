using Guppy.DependencyInjection;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Driver
    {
        protected Object driven { get; private set; }

        #region Lifecycle Methods
        /// <summary>
        /// The "Initialize" equivalent for Drivers. This will automatically be called
        /// by the containing driven instance on creation (which takes place within
        /// Driven.PostInitialize()).
        /// </summary>
        /// <param name="driven"></param>
        /// <param name="provider"></param>
        internal virtual void TryConfigure(Object driven, ServiceProvider provider)
        {
            this.Configure(driven, provider);
        }
        protected virtual void Configure(Object driven, ServiceProvider provider)
        {
            this.driven = driven;
        }

        internal virtual void TryDispose()
        {
            this.Dispose();
        }

        protected virtual void Dispose()
        {

        }
        #endregion
    }
    public class Driver<T> : Driver
        where T : Driven
    {
        protected new T driven { get; private set; }

        protected override void Configure(Object driven, ServiceProvider provider)
        {
            ExceptionHelper.ValidateAssignableFrom<T>(driven.GetType());

            this.driven = driven as T;
        }
    }
}
