﻿using Guppy.Interfaces;
using Guppy.Utilities.Loaders;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;
using Guppy.Utilities.Factories;
using Guppy.Collections;
using Guppy.Extensions.Collection;

namespace Guppy.Implementations
{
    public class Driven : Frameable
    {
        #region Public Attributes
        public FrameableCollection<Driver> Drivers { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            // Load new drivers for the current object
            this.Drivers = this.provider.GetService<DriverFactory>().Pull(this);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.provider.GetService<DriverFactory>().Put(this.Drivers);
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
