using Guppy.Common.DependencyInjection;
using Guppy.GUI.Providers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Loaders
{
    internal sealed class GUIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddService<IStyleSheetProvider>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetImplementationType<StyleSheetProvider>();

            services.AddService<Stage>()
                .SetLifetime(ServiceLifetime.Transient);

            services.ConfigureCollection(manager =>
            {
                manager.AddScoped<IScreen>().SetImplementationType<Screen>();
            });
            
        }
    }
}
