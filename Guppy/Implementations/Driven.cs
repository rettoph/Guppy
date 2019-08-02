using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Implementations
{
    public class Driven : Reusable, IDriven
    {
        #region Private Fields
        private IDriver[] _drivers;
        private IDriver[] _drawDrivers;
        #endregion

        #region Lifecycle Methods
        protected override void PreCreate(IServiceProvider provider)
        {
            base.PreCreate(provider);

            // Create new driver instances for the current driven
            _drivers = provider.GetDrivers(this);
            _drawDrivers = _drivers.OrderBy(d => d.DrawOrder).ToArray();
        }

        protected override void PostCreate(IServiceProvider provider)
        {
            base.PostCreate(provider);

            // Call the driver create method
            foreach (IDriver driver in _drivers)
                driver.TryCreate(provider);
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            foreach (IDriver driver in _drivers)
                driver.TryPreInitialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            foreach (IDriver driver in _drivers)
                driver.TryInitialize();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            foreach (IDriver driver in _drivers)
                driver.TryPostInitialize();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (IDriver driver in _drawDrivers)
                driver.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (IDriver driver in _drivers)
                driver.TryUpdate(gameTime);

            base.Update(gameTime);
        }
        #endregion

        #region Helper Methods
        public TDriver GetFirstDriver<TDriver>() 
            where TDriver : class, IDriver
        {
            return _drivers.First(d => typeof(TDriver).IsAssignableFrom(d.GetType())) as TDriver;
        }

        public IEnumerable<IDriver> GetDrivers<TDriver>() 
            where TDriver : class, IDriver
        {
            return _drivers.Where(d => typeof(TDriver).IsAssignableFrom(d.GetType()));
        }
        #endregion
    }
}
