using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Attributes;
using Guppy.Extensions;
using Guppy.Extensions.Linq;
using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Loaders
{
    [IsLoader(90)]
    public class DriverLoader : Loader<Type, Type, IEnumerable<Type>>
    {
        private IServiceProvider _provider;
        private HashSet<Type> _rawDriverTypes;

        public DriverLoader(IServiceProvider provider, ILogger logger) : base(logger)
        {
            _provider = provider;
            _rawDriverTypes = new HashSet<Type>();
        }

        #region Registration Methods
        public void TryRegister<TDriven, TDriver>()
            where TDriven : Driven
            where TDriver : Driver
        {
            this.TryRegister(typeof(TDriven), typeof(TDriver));
        }
        public void TryRegister(Type drivenType, Type driverType)
        {
            if (!typeof(Driven).IsAssignableFrom(drivenType))
                throw new Exception($"Unable to register Driver. Type<{drivenType.Name}> does not extend Driven.");
            else if (!typeof(Driver).IsAssignableFrom(driverType))
                throw new Exception($"Unable to register Driver. Type<{driverType.Name}> does not extend Driver.");

            // Register the driver type...
            this.Register(drivenType, driverType);
            _rawDriverTypes.Add(driverType);
        }
        #endregion

        public override void Load()
        {
            base.Load();


            // At this time, ensure that all of the internal raw 
            // driver types are given pools within the pool loader
            var poolLoader = _provider.GetService<PoolLoader>();
            _rawDriverTypes.ForEach(t => poolLoader.TryRegisterInitializable(t));
        }

        protected override Dictionary<Type, IEnumerable<Type>> BuildValuesTable()
        {
            return new Dictionary<Type, IEnumerable<Type>>();
        }

        public override IEnumerable<Type> GetValue(Type handle)
        {
            if (!this.valuesTable.ContainsKey(handle))
            { // If the drivers for the specified type have not been configured yet...
                // Load all drivers bound to a type assignable to the input type...
                return this.valuesTable[handle] = this.registeredValuesList
                    .Where(rv => rv.Handle.IsAssignableFrom(handle))
                    .Select(rv => rv.Value);
            }

            // Return the base value...
            return base.GetValue(handle);
        }
    }
}
