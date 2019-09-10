using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy
{
    public abstract class Driver
    {
        #region Protected Attributes
        protected ILogger logger { get; private set; }
        #endregion

        #region Constructor
        public Driver(Driven driven)
        {
        }
        #endregion

        #region Driven Interfaces
        internal void TryCreate(IServiceProvider provider)
        {
            this.Create(provider);
        }

        internal void TryPreInitialize()
        {
            this.PreInitialize();
        }

        internal void TryInitialize()
        {
            this.Initialize();
        }

        internal void TryPostInitialize()
        {
            this.PostInitialize();
        }

        internal void TryDispose()
        {
            this.Dispose();
        }

        internal void TryDraw(GameTime gameTime)
        {
            this.Draw(gameTime);
        }

        internal void TryUpdate(GameTime gameTime)
        {
            this.Update(gameTime);
        }
        #endregion

        #region Lifecycle Methods
        protected virtual void Create(IServiceProvider provider)
        {
            this.logger = provider.GetRequiredService<ILogger>();
        }

        protected virtual void PreInitialize()
        {
            //
        }

        protected virtual void Initialize()
        {
            //
        }

        protected virtual void PostInitialize()
        {
            //
        }

        protected virtual void Dispose()
        {
            //
        }
        #endregion

        #region Frame Methods
        protected virtual void Draw(GameTime gameTime)
        {
            //
        }

        protected virtual void Update(GameTime gameTime)
        {
            //
        }
        #endregion
    }

}