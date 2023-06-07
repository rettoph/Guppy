using Guppy.Common.DependencyInjection;
using Guppy.Input.Providers;
using Guppy.Loaders;
using Guppy.Input.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Loaders
{
    internal sealed class InputLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCollection(manager =>
            {
                manager.GetService<ButtonService>()
                    .SetLifetime(ServiceLifetime.Scoped)
                    .AddAlias<IButtonService>()
                    .AddAlias<IGameComponent>();

                manager.GetService<KeyboardButtonProvider>()
                    .SetLifetime(ServiceLifetime.Scoped)
                    .AddAlias<IButtonProvider>();

                manager.GetService<MouseButtonProvider>()
                    .SetLifetime(ServiceLifetime.Scoped)
                    .AddAlias<IButtonProvider>();

                manager.GetService<MouseEventPublishService>()
                    .SetLifetime(ServiceLifetime.Scoped)
                    .AddAlias<IGameComponent>();

                manager.GetService<ICursorProvider>()
                    .SetLifetime(ServiceLifetime.Scoped)
                    .SetImplementationType<CursorProvider>();
            });
        }
    }
}
