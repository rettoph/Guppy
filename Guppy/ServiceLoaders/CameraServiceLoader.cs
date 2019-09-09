using Guppy.Attributes;
using Guppy.Extensions.Collection;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    internal sealed class CameraServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<CameraFactory>();

            // Auto register any cameras
            AssemblyHelper.GetTypesAssignableFrom<Camera>()
                .Where(t => t.IsClass && !t.IsAbstract)
                .ForEach(t => services.AddTransient(t, p => p.GetRequiredService<CameraFactory>().Build(t)));
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
