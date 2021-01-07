using Guppy.Extensions.System.Collections;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Enums;
using Guppy.Utilities;
using Guppy.DependencyInjection;
using System.Linq;
using Guppy.Extensions.DependencyInjection;
using Guppy.Services;

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
        #region Private Fields
        private List<Driver> _drivers;
        private DriverService _driverService;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            provider.Service(out _driverService);

            _drivers = new List<Driver>();
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _driverService.BuildDrivers(this, provider, ref _drivers);
        }

        protected override void Release()
        {
            base.Release();

            _driverService.ReleaseDrivers(this, ref _drivers);
        }

        protected override void Dispose()
        {
            base.Dispose();

            _driverService = null;
        }
        #endregion
    }
}
