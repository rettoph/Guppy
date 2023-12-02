using Autofac;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Filters;
using Guppy.Extensions.Autofac;
using Guppy.Game.Common;
using Guppy.Loaders;
using Serilog;
namespace Guppy.Game
{
    [AutoLoad]
    internal sealed class GameLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Game>().As<IGame>().SingleInstance();

            services.RegisterType<ConsoleTerminal>().AsImplementedInterfaces().AsSelf().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);
            services.RegisterType<TerminalTheme>().As<ITerminalTheme>().SingleInstance();

            services.Configure<LoggerConfiguration>((scope, config) =>
            {
                config.MinimumLevel.ControlledBy(LogLevelCommand.LoggingLevelSwitch);
            });
        }
    }
}
