using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Configuration;
using Guppy.Core.Resources.Extensions.Autofac;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Common.Loaders;
using Guppy.Game.Common;
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
