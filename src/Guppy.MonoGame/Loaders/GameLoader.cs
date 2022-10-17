using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Providers;
using Guppy.MonoGame.Services;
using Guppy.MonoGame.Systems;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<ITerminalService, ConsoleTerminalService>();
            services.AddScoped<IDebuggerService, DefaultDebuggerService>();

            services.AddSystem<TerminalSystem>(SystemConstants.DefaultOrder);
            services.AddSystem<DebuggerSystem>(SystemConstants.DefaultOrder);
            services.AddSystem<GlobalSystem>(SystemConstants.DefaultOrder);
        }
    }
}
