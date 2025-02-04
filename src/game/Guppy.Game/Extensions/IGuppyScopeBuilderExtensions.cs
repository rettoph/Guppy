using Guppy.Core.Commands.Extensions;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Common;
using Guppy.Core.Logging.Common.Sinks;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Engine.Providers;
using Guppy.Engine.Systems.Guppy;
using Guppy.Game.Common;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Common.Services;
using Guppy.Game.ResourceTypes;
using Guppy.Game.Serialization.Json.Converters;
using Guppy.Game.Serilog.Sinks;
using Guppy.Game.Services;
using Guppy.Game.Systems.Engine;
using Guppy.Game.Systems.Guppy;

namespace Guppy.Game.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCommonGameServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCommonGameServices), builder =>
            {
                builder.RegisterCoreCommandServices();

                builder.RegisterType<SceneStateProvider>().As<IStateProvider>().InstancePerLifetimeScope();

                builder.RegisterJsonConverter<ColorConverter>();
                builder.RegisterJsonConverter<Vector2Converter>();
                builder.RegisterJsonConverter<Vector3Converter>();

                builder.RegisterType<SceneService>().As<ISceneService>().SingleInstance();

                builder.RegisterType<TerminalTheme>().As<ITerminalTheme>().SingleInstance();
                builder.RegisterType<TerminalLogMessageSink>().As<ILogMessageSink>().InstancePerLifetimeScope();

                builder.RegisterGlobalSystem<SceneFrameSystem>();

                builder.RegisterSceneFilter<IScene>(builder =>
                {
                    builder.RegisterSceneSystem<SceneBrokerSystem>();
                    builder.RegisterSceneSystem<SceneBusSystem>();
                });

                builder.RegisterResourceType<ColorResourceType>();
                builder.RegisterResourcePack(new ResourcePackConfiguration()
                {
                    EntryDirectory = DirectoryLocation.CurrentDirectory(GuppyGamePack.Directory)
                });
            });
        }
    }
}