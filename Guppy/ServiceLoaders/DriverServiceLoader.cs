using Guppy.Attributes;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Simple service loader used to dynamically load 
    /// all driver types with the DriverAttribute defined
    /// and automatically register them to the service
    /// collection.
    /// </summary>
    public class DriverServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            foreach(Type driverType in AssemblyHelper.GetTypesWithAttribute<IsDriverAttribute>())
            { // Iterate through all types that contain a driver attribute
                if (!typeof(IDriver).IsAssignableFrom(driverType))
                    throw new Exception($"Invalid type with Driver attribute! {driverType.Name}");

                // Register the types drivers
                foreach(IsDriverAttribute attribute in driverType.GetCustomAttributes(typeof(IsDriverAttribute), false))
                    services.AddDriver(attribute.DrivenType, driverType);
            }
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
