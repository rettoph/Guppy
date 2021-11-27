using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Threading.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<ThreadQueue>(p => new ThreadQueue());
            services.RegisterTypeFactory<IntervalInvoker>(p => new IntervalInvoker());

            services.RegisterService(Constants.ServiceConfigurationKeys.GameUpdateThreadQueue)
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory<ThreadQueue>();

            services.RegisterService(Constants.ServiceConfigurationKeys.SceneUpdateThreadQueue)
                .SetLifetime(ServiceLifetime.Scoped)
                .SetTypeFactory<ThreadQueue>();

            services.RegisterService<IntervalInvoker>()
                .SetLifetime(ServiceLifetime.Scoped);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
