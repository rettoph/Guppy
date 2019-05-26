using Guppy.Extensions.DependencyInjection;
using Guppy.Implementations;
using Guppy.Interfaces;
using Guppy.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    /// <summary>
    /// Driven classes contain drivers defined within
    /// the DriverLoader.
    /// </summary>
    public class Driven : Frameable, IDriven
    {
        private Driver[] _drivers;

        public Driven(IServiceProvider provider, ILogger logger) : base(logger)
        {
            _drivers = provider.GetDrivers(this);
        }

        public Driven(Guid id, IServiceProvider provider, ILogger logger) : base(id, logger)
        {
            _drivers = provider.GetDrivers(this);
        }

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            foreach (Driver driver in _drivers)
                driver.TryBoot();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            foreach (Driver driver in _drivers)
                driver.TryPreInitialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            foreach (Driver driver in _drivers)
                driver.TryInitialize();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            foreach (Driver driver in _drivers)
                driver.TryPostInitialize();
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
            foreach (Driver driver in _drivers)
                driver.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Driver driver in _drivers)
                driver.Update(gameTime);
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            foreach (Driver driver in _drivers)
                driver.Dispose();
        }
    }
}
