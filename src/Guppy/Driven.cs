using Guppy.Extensions.Collections;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Enums;
using Guppy.Utilities;
using Guppy.DependencyInjection;
using System.Linq;
using Guppy.Extensions.DependencyInjection;

namespace Guppy
{
    /// <summary>
    /// Defines objects that will automatically load
    /// registered drivers (or recieve drivers of its own
    /// on custom configuration).
    /// 
    /// Drivers should contain custom additional or optional
    /// implementations.
    /// </summary>
    public class Driven : Frameable
    {
        #region Static Fields
        #endregion
        #region Private Fields
        private HashSet<Type> _driverTypes;
        private Driver[] _drivers;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _driverTypes = new HashSet<Type>();
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            // For each registered type create a new instance with the custom setup
            _drivers = _driverTypes.Select(d =>
            {
                var driver = (Driver)provider.GetService(d);
                driver.TryConfigure(this, provider);
                return driver;
            }).ToArray();
        }

        protected override void Dispose()
        {
            base.Dispose();

            _drivers.ForEach(d => d.TryDispose());
        }
        #endregion

        #region Helper Methods
        public void AddDriver(Type driver)
        {
            if (this.InitializationStatus >= InitializationStatus.PostInitializing)
                throw new InvalidOperationException("Unable to add driver post initialization.");

            ExceptionHelper.ValidateAssignableFrom<Driver>(driver);

            _driverTypes.Add(driver);
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _drivers.ForEach(d => d.TryUpdate(gameTime));
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _drivers.ForEach(d => d.TryDraw(gameTime));
        }
        #endregion
    }
}
