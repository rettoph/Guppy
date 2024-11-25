using Autofac;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Commands.Extensions;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Engine.Components.Guppy;
using Guppy.Engine.Providers;
using Guppy.Game.Common;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Common.Services;
using Guppy.Game.Components.Engine;
using Guppy.Game.Components.Guppy;
using Guppy.Game.Components.Scene;
using Guppy.Game.ResourceTypes;
using Guppy.Game.Serialization.Json.Converters;
using Guppy.Game.Services;
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

                builder.RegisterType<SceneStateProvider>().As<IStateProvider>().InstancePerLifetimeScope();

                builder.RegisterType<ColorConverter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<Vector2Converter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<Vector3Converter>().As<JsonConverter>().SingleInstance();

                builder.RegisterType<SceneService>().As<ISceneService>().SingleInstance();

                builder.RegisterType<TerminalTheme>().As<ITerminalTheme>().SingleInstance();

                builder.RegisterType<SceneFrameComponent>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<EngineLogLevelComponent>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<SceneBrokerComponent>().AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterType<SceneBusComponent>().AsImplementedInterfaces().InstancePerLifetimeScope();

                builder.RegisterSceneFilter<ICommandContext<LogLevelCommand>>(null);

                builder.RegisterResourceType<ColorResourceType>();
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
