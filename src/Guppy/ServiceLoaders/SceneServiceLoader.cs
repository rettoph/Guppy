﻿using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class SceneServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<SceneCollection>();

            AssemblyHelper.GetTypesAssignableFrom<Scene>().ForEach(t =>
            { // Auto register any Scene classes as a scoped type.
                services.AddTypedScoped(t, typeof(Scene));
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
