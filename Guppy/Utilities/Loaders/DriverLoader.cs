using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Attributes;
using Guppy.Extensions;
using Guppy.Extensions.Collection;
using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;

namespace Guppy.Utilities.Loaders
{
    [IsLoader(90)]
    public class DriverLoader : Loader<Type, Type, IEnumerable<Type>>
    {
        private PoolLoader _poolLoader;
        private HashSet<Type> _rawDriverTypes;

        public DriverLoader(PoolLoader pools, ILogger logger) : base(logger)
        {
            _poolLoader = pools;
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
            ExceptionHelper.ValidateAssignableFrom<Driven>(drivenType);
            ExceptionHelper.ValidateAssignableFrom<Driver>(driverType);

            this.logger.LogTrace($"Registering new Driver<{driverType.Name}> => '{drivenType.Name}'");

            // Register the driver type...
            this.Register(drivenType, driverType);
            _rawDriverTypes.Add(driverType);
            _poolLoader.TryRegisterInitializable(driverType);
        }
        #endregion

        public override void Load()
        {
            base.Load();
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
