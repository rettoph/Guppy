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
        #region Static Fields
        #endregion

        #region Private Fields
        private Driver[] _drivers;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            _drivers = provider.GetService<DriverService>().BuildDrivers(this, provider);

            _drivers.ForEach(d => d.TryCreate(this, provider));
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _drivers.ForEach(d => d.TryPreInitialize(this, provider));
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _drivers.ForEach(d => d.TryInitialize(this, provider));
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            _drivers.ForEach(d => d.TryPostInitialize(this, provider));
        }

        protected override void Release()
        {
            base.Release();

            _drivers.ForEach(d => d.TryRelease(this));
        }

        protected override void Dispose()
        {
            base.Dispose();

            _drivers.ForEach(d => d.TryDispose(this));
        }
        #endregion
    }
}
