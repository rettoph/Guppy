using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Extensions.Linq;
using Guppy.Extensions.DependencyInjection;
using Guppy.Implementations;
using Guppy.Loaders;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    public class DriverServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // throw new NotImplementedException();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            var loader = provider.GetService<DriverLoader>();

            // Automatically register all Driver types with the IsDriver attribute
            AssemblyHelper.GetTypesWithAttribute<Driver, IsDriverAttribute>().ForEach(driver =>
            {
                driver.GetCustomAttributes(typeof(IsDriverAttribute), true).Select(o => o as IsDriverAttribute).ForEach(attribute =>
                {
                    loader.TryRegister(attribute.Driven, driver);
                });
            });
        }
    }
}
