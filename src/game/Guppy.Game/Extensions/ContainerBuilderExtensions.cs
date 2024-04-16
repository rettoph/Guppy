using Autofac;
using Guppy.Core.Commands.Extensions;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions.Autofac;
using Guppy.Game.Common;
using Guppy.Game.Serialization.Json.Converters;
using Serilog;
using System.Text.Json.Serialization;

namespace Guppy.Game.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCommonGameServices(this ContainerBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCommonGameServices), builder =>
            {
                builder.RegisterCoreCommandServices();

                builder.RegisterType<ColorConverter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<Vector2Converter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<Vector3Converter>().As<JsonConverter>().SingleInstance();

                builder.RegisterType<Game>().As<IGame>().SingleInstance();

                builder.RegisterType<TerminalTheme>().As<ITerminalTheme>().SingleInstance();

                builder.RegisterResourcePack(new ResourcePackConfiguration()
                {
                    EntryDirectory = DirectoryLocation.CurrentDirectory(GuppyGamePack.Directory)
                });

                builder.Configure<LoggerConfiguration>((scope, config) =>
                {
                    config.MinimumLevel.ControlledBy(LogLevelCommand.LoggingLevelSwitch);
                });
            });
        }
    }
}
