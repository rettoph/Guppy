using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class SceneServiceLoader : IServiceLoader
    {
        public void ConfigureServices(DependencyInjection.ServiceCollection services)
        {
            services.AddFactory<SceneList>(p => new SceneList());
            services.AddSingleton<SceneList>();

            AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<Scene>(false).ForEach(s =>
            {
                services.AddScene(s, p => ActivatorUtilities.CreateInstance(p, s));
            });
        }

        public void ConfigureProvider(DependencyInjection.ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
