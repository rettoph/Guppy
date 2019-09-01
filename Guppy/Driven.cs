using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;
using Guppy.Extensions.Collection;
using Guppy.Collections;

namespace Guppy
{
    public abstract class Driven : Frameable
    {
        #region Internal Fields
        internal IEnumerable<Driver> drivers;
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.drivers.ForEach(d => d.TryCreate(provider));
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.drivers.ForEach(d => d.TryPreInitialize());
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.drivers.ForEach(d => d.TryInitialize());
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.drivers.ForEach(d => d.TryPostInitialize());
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw all drivers...
            this.drivers.ForEach(d => d.TryDraw(gameTime));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update all drivers...
            this.drivers.ForEach(d => d.TryUpdate(gameTime));
        }
        #endregion

        #region Helper Methods
        public TDriver GetDriver<TDriver>() 
            where TDriver : Driver
        {
            return this.drivers.FirstOrDefault(d => typeof(TDriver).IsAssignableFrom(d.GetType())) as TDriver;
        }
        #endregion
    }
}
