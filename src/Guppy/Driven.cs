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
using Microsoft.Extensions.DependencyInjection;
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
        private Driver[] _drivers;
        private DriverService _driverService;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            provider.Service(out _driverService);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _drivers = _driverService.BuildDrivers(this, provider);
        }

        protected override void Release()
        {
            base.Release();

            _driverService.ReleaseDrivers(this, ref _drivers);
        }
        #endregion
    }
}
