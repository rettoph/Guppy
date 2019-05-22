using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Loaders
{
    public class DriverLoader : Loader<Type, Type, Type[]>
    {
        private IServiceProvider _provider;

        public DriverLoader(IServiceProvider provider, ILogger logger) : base(logger)
        {
            _provider = provider;
        }

        protected override Dictionary<Type, Type[]> BuildValuesTable()
        {
            return this.registeredValuesList
                .OrderBy(rv => rv.Priority)
                .GroupBy(rv => rv.Handle)
                .ToDictionary(
                    keySelector: g => g.First().Handle,
                    elementSelector: g => g.Select(rv => rv.Value).ToArray());
        }

        public override void Register(Type handle, Type value, ushort priority = 0)
        {
            if (!typeof(Driven).IsAssignableFrom(handle))
                throw new Exception("Unable to register Driver. Invalid Driven type.");
            else if (!typeof(Driver).IsAssignableFrom(value))
                throw new Exception("Unable to register Driver. Invalid Driver type.");

            base.Register(handle, value, priority);
        }

        public void Register<TDriven, TDriver>(ushort priority = 0)
            where TDriven : Driven
            where TDriver : Driver
        {
            this.Register(typeof(TDriven), typeof(TDriver), priority);
        }

        public Driver[] BuildDrivers(Driven driven)
        {
            return this.GetValue(driven.GetType())
                .Select(dt => (Driver)ActivatorUtilities.CreateInstance(_provider, dt, driven))
                .ToArray();
        }
    }
}
