using Guppy.Interfaces;
using Guppy.Loaders;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;
using Guppy.Utilities.Factories;

namespace Guppy.Implementations
{
    public class Driven : Frameable
    {
        #region Public Attributes
        public IEnumerable<Driver> Drivers { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            // Load new drivers for the current object
            this.Drivers = this.provider.GetService<DriverFactory>().GetDrivers(this);
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw all drivers...
            foreach (Driver driver in this.Drivers)
                driver.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update all drivers...
            foreach (Driver driver in this.Drivers)
                driver.TryUpdate(gameTime);
        }
        #endregion

        #region Helper Methods
        public TDriver GetDriver<TDriver>() 
            where TDriver : Driver
        {
            return this.Drivers.FirstOrDefault(d => typeof(TDriver).IsAssignableFrom(d.GetType())) as TDriver;
        }
        #endregion
    }
}
