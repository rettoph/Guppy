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
        internal FrameableCollection<Driver> drivers;
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Create a new driver collection...
            this.drivers = provider.GetService<FrameableCollection<Driver>>();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.drivers.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw all drivers...
            this.drivers.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update all drivers...
            this.drivers.TryUpdate(gameTime);
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
