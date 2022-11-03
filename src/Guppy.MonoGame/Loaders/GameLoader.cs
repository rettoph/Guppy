using Guppy.Common;
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
            services.AddSingleton<ConsoleTerminalService>()
                .AddAlias(Alias.Create<ITerminalService, ConsoleTerminalService>());
            services.AddSingleton<DefaultDebuggerService>()
                .AddAlias(Alias.Create<IDebuggerService, DefaultDebuggerService>());

            services.AddSystem<TerminalSystem>()
                .AddSystem<DebuggerSystem>()
                .AddSystem<GlobalSystem>();
        }
    }
}
