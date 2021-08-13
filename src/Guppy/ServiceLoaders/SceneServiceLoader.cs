using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
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
        public void RegisterServices(DependencyInjection.GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<SceneList>(p => new SceneList());
            services.RegisterSingleton<SceneList>();

            AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<IScene>(false).ForEach(s =>
            {
                services.RegisterScene(s);
            });
        }

        public void ConfigureProvider(DependencyInjection.GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
