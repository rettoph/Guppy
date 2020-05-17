using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad(Int32.MaxValue)]
    internal class ServiceInitializationServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddConfiguration<IService>((s, p, c) =>
            {
                s.ServiceConfiguration = c;
                s.TryPreInitialize(p);
            }, -10);

            services.AddConfiguration<IService>((s, p, f) => s.TryInitialize(p), 10);

            services.AddConfiguration<IService>((s, p, f) => s.TryPostInitialize(p), 20);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
