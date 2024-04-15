using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Extensions.Autofac;
using Guppy.Core.Files;
using Guppy.Game.Common;
using Guppy.Engine.Loaders;
using Guppy.Core.Resources.Configuration;
using Guppy.Core.Resources.Extensions.Autofac;
using Serilog;
namespace Guppy.Game
{
    [AutoLoad]
    internal sealed class GameLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Game>().As<IGame>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.EngineScope);

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
