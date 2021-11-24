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

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<ThreadQueue>(p => new ThreadQueue());
            services.RegisterSingleton(Constants.ServiceConfigurationKeys.GameUpdateThreadQueue, typeof(ThreadQueue));
            services.RegisterScoped(Constants.ServiceConfigurationKeys.SceneUpdateThreadQueue, typeof(ThreadQueue));

            services.RegisterTypeFactory<IntervalInvoker>(p => new IntervalInvoker());
            services.RegisterScoped<IntervalInvoker>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
