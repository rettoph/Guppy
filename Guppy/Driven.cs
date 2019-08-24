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
        #region Public Attributes
        public FrameableCollection<Driver> Drivers { get; internal set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Create a new driver collection...
            this.Drivers = provider.GetService<FrameableCollection<Driver>>();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Drivers.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw all drivers...
            this.Drivers.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update all drivers...
            this.Drivers.TryUpdate(gameTime);
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
