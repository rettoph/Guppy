using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Service loader used to automatically register all
    /// game types into the service collection.
    /// </summary>
    [IsServiceLoader]
    internal sealed class SceneServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SceneFactory>();
            services.AddSingleton<SceneCollection>();

            AssemblyHelper.GetTypesAssignableFrom<Scene>().ForEach(t =>
            { // Add each scene type as a singleton created via the scene factory...
                services.AddSingleton(t, p => p.GetRequiredService<SceneFactory>().Build(t));
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
