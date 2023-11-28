using Autofac;
using Guppy.Common;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Filters;
using Guppy.Files.Enums;
using Guppy.Files.Helpers;
using Guppy.Files.Providers;
using Guppy.Loaders;
using Guppy.MonoGame.Commands;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Components;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Providers;
using Microsoft.Xna.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class GameLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Game>().As<IGame>().SingleInstance();
            services.RegisterType<MenuProvider>().As<IMenuProvider>().InstancePerLifetimeScope();

            services.AddFilter(new ServiceFilter<IGameComponent, Type>(typeof(MonoGameGuppy)));

            services.Configure<LoggerConfiguration>((scope, config) =>
            {
                config.MinimumLevel.ControlledBy(LogLevelCommand.LoggingLevelSwitch);
            });
        }
    }
}
