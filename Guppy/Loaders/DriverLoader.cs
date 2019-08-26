﻿using Guppy.Attributes;
using Guppy.Extensions.Collection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    /// <summary>
    /// Class that will automatically load all drivers
    /// and create a dictionary of types and the driver
    /// types assigned to them.
    /// </summary>
    internal sealed class DriverLoader : Loader<Type, Type, Type[]>
    {
        #region Constructor
        public DriverLoader(ILogger logger) : base(logger)
        {
        }
        #endregion

        #region ILoader Implementation
        public override void Load()
        {
            // Load a list of all driven types that are not abstract...
            var drivens = AssemblyHelper.GetTypesAssignableFrom<Driven>().Where(t => t.IsClass && !t.IsAbstract);
            
            // Load all drivers...
            AssemblyHelper.GetTypesWithAttribute<Driver, IsDriverAttribute>().ForEach(driver =>
            { // Iterate through all driver types...
                this.logger.LogTrace($"Loading Driver<{driver.Name}> configurations...");
                driver.GetCustomAttributes(typeof(IsDriverAttribute), false).Select(attr => attr as IsDriverAttribute).ForEach(attribute =>
                { // Iterate through all attributes within the current driver...
                    drivens.Where(driven => attribute.Driven.IsAssignableFrom(driven)).ForEach(driven =>
                    { // Iterate through all types that qualify for the current driver...
                        // Add the driver type to the output driver table...
                        this.Register(driven, driver, attribute.Priority);
                    });
                });
            });

            // Configure the values table...
            base.Load();

            // Ensure that a value gets defined for every driven type
            this.logger.LogTrace($"    - Adding driver placeholders...");
            var emptyDrivers = new Type[0];
            drivens.ForEach(driven =>
            {
                if (!this.values.ContainsKey(driven))
                    this.values[driven] = emptyDrivers;
            });

            this.logger.LogTrace($"    - {this.values.Count} values cached.");
        }
        #endregion

        protected override Type[] BuildOutput(IGrouping<Type, RegisteredValue> input)
        {
            this.logger.LogTrace($"Building Driven<{input.Key}> driver configuration...");
            return input.OrderBy(rv => rv.Priority).Select(rv => rv.Value).ToArray();
        }
    }
}