using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Extensions.Autofac;
using Guppy.Files.Enums;
using Guppy.Game.Common;
using Guppy.Loaders;
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

            services.Configure<LoggerConfiguration>((scope, config) =>
            {
                config.MinimumLevel.ControlledBy(LogLevelCommand.LoggingLevelSwitch);
            });

            services.ConfigureResourcePacks((scope, packs) =>
            {
                packs.Add(FileType.CurrentDirectory, Path.Combine(GuppyGamePack.Directory, "pack.json"));
            });
        }
    }
}
