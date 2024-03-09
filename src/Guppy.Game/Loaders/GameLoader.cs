using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Extensions.Autofac;
using Guppy.Files;
using Guppy.Game.Common;
using Guppy.Loaders;
using Guppy.Resources.Configuration;
using Guppy.Resources.Extensions.Autofac;
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

            services.RegisterResourcePack(new ResourcePackConfiguration()
            {
                EntryDirectory = DirectoryLocation.CurrentDirectory(GuppyGamePack.Directory)
            });

            services.Configure<LoggerConfiguration>((scope, config) =>
            {
                config.MinimumLevel.ControlledBy(LogLevelCommand.LoggingLevelSwitch);
            });
        }
    }
}
