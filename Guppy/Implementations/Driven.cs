using Guppy.Extensions.DependencyInjection;
using Guppy.Implementations;
using Guppy.Interfaces;
using Guppy.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Utilities.DynamicDelegaters;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Implementations
{
    /// <summary>
    /// Driven classes contain drivers defined within
    /// the DriverLoader.
    /// </summary>
    public abstract class Driven : Frameable, IDriven
    {
        private Driver[] _drivers;

        /// <summary>
        /// Should manage all event ran by the current driven object.
        /// 
        /// Custom handlers can be added at any time.
        /// </summary>
        public EventDelegater Events { get; private set; }

        public Driven(IServiceProvider provider) : base(provider)
        {
            _drivers = provider.GetDrivers(this);
        }

        public Driven(Guid id, IServiceProvider provider) : base(id, provider)
        {
            _drivers = provider.GetDrivers(this);
        }

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            this.Events = ActivatorUtilities.CreateInstance<EventDelegater>(
                this.provider, 
                (Object)this);

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
        public new void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (Driver driver in _drivers)
                driver.draw(gameTime);
        }

        public new void Update(GameTime gameTime)
        {
            foreach (Driver driver in _drivers)
                driver.update(gameTime);

            base.Update(gameTime);
        }
        #endregion

        /// <summary>
        /// Return all internal drivers that are assignable from
        /// a given type
        /// </summary>
        /// <typeparam name="TDriver"></typeparam>
        /// <returns></returns>
        public IEnumerable<TDriver> GetDrivers<TDriver>()
            where TDriver : Driver
        {
            return _drivers
                .Where(d => typeof(TDriver).IsAssignableFrom(d.GetType()))
                .Select(d => d as TDriver);
        }

        /// <summary>
        /// Return the first driver that is assignable
        /// from the given type
        /// </summary>
        /// <typeparam name="TDriver"></typeparam>
        /// <returns></returns>
        public TDriver GetDriver<TDriver>()
            where TDriver : Driver
        {
            return _drivers
                .FirstOrDefault(d => typeof(TDriver).IsAssignableFrom(d.GetType())) as TDriver;
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (Driver driver in _drivers)
                driver.Dispose();
        }
    }
}
