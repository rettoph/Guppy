using Guppy.Common;
using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.GameComponents;
using Guppy.MonoGame.Providers;
using Guppy.MonoGame.Services;
using Guppy.MonoGame.Strategies.PublishStrategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class GameLoader<TGlobalBrokerPublishStrategy> : IServiceLoader
        where TGlobalBrokerPublishStrategy : PublishStrategy
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGlobalBroker, GlobalBroker<TGlobalBrokerPublishStrategy>>()
                .AddSingleton<TGlobalBrokerPublishStrategy>();

            services.AddScoped<DefaultDebuggerService>()
                .AddAlias(Alias.Create<IDebuggerService, DefaultDebuggerService>());

            services.AddScoped<ConsoleTerminalService>()
                .AddAlias(Alias.Create<ITerminalService, ConsoleTerminalService>());

            services.AddScoped<IGameComponentService, GameComponentService>();

            services.AddScoped<IGameComponent, BusGameComponent>();
        }
    }
}
