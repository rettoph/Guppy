using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    internal sealed class ContentServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<ContentService>(p => new ContentService());

            services.RegisterSingleton<ContentService>();

            services.RegisterSetup<ContentService>((content, p, c) =>
            {
                content.TryRegister(Constants.Content.DebugFont, "Fonts/DiagnosticsFont");
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
