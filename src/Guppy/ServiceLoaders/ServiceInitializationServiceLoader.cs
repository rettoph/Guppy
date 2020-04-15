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
            services.ServiceDescriptors
                .Where(s => typeof(IService).IsAssignableFrom(s.ServiceType))
                .Select(s => s.ServiceType)
                .Distinct()
                .ForEach(s =>
                { // Auto register the initialization methods to all services implementing the IService interface.
                    services.ConfigurationDescriptors.Add(new ConfigurationDescriptor()
                    { // PreInitialize
                        Name = String.Empty,
                        ServiceType = s,
                        Priority = -10,
                        Configure = (i, p, c) =>
                        {
                            ((IService)i).Configuration = c;
                            ((IService)i).TryPreInitialize(p);
                            return i;
                        },
                    });

                    services.ConfigurationDescriptors.Add(new ConfigurationDescriptor()
                    { // Initialize
                        Name = String.Empty,
                        ServiceType = s,
                        Priority = 10,
                        Configure = (i, p, c) =>
                        {
                            ((IService)i).TryInitialize(p);
                            return i;
                        },
                    });

                    services.ConfigurationDescriptors.Add(new ConfigurationDescriptor()
                    { // PostInitialize
                        Name = String.Empty,
                        ServiceType = s,
                        Priority = 20,
                        Configure = (i, p, c) =>
                        {
                            ((IService)i).TryPostInitialize(p);
                            return i;
                        },
                    });
                });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
