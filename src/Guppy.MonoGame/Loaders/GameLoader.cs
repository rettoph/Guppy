using Guppy.Common;
using Guppy.Common.DependencyInjection;
using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Providers;
using Guppy.MonoGame.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class GameLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddService<ConsoleTerminalService>()
                .SetLifetime(ServiceLifetime.Scoped)
                .AddAlias<ITerminalService>();

            services.AddScoped<IGameComponentService, GameComponentService>();

            services.AddScoped<IMenuProvider, MenuProvider>();
            services.AddScoped<IMenuLoader, DebugMenuLoader>();

            services.AddSingleton<ICommandService, CommandService>();

            services.AddSingleton<IEngineLoader, GuppyProviderLoader>();
        }
    }
}
