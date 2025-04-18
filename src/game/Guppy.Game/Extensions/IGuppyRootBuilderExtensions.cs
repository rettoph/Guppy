﻿using Autofac;
using Guppy.Core.Commands.Extensions;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Common;
using Guppy.Core.Logging.Common.Sinks;
using Guppy.Core.Assets.Common.Configuration;
using Guppy.Core.Assets.Common.Extensions;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Game.Common;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Common.Services;
using Guppy.Game.AssetTypes;
using Guppy.Game.Serialization.Json.Converters;
using Guppy.Game.Serilog.Sinks;
using Guppy.Game.Services;
using Guppy.Game.Systems.Engine;
using Guppy.Game.Systems.Scene;

namespace Guppy.Game.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCommonGameServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCommonGameServices), builder =>
            {
                builder.RegisterCoreCommandServices();

                builder.RegisterGlobalSystem<EngineBusSystem>();

                builder.RegisterJsonConverter<ColorConverter>();
                builder.RegisterJsonConverter<Vector2Converter>();
                builder.RegisterJsonConverter<Vector3Converter>();

                builder.RegisterType<SceneService>().AsSelf().As<ISceneService>().SingleInstance();

                builder.RegisterType<TerminalTheme>().As<ITerminalTheme>().SingleInstance();
                builder.RegisterType<TerminalLogMessageSink>().As<ILogMessageSink>().InstancePerLifetimeScope();

                builder.RegisterGlobalSystem<SceneFrameSystem>();

                builder.RegisterSceneFilter<IScene>(builder =>
                {
                    builder.RegisterSceneSystem<SceneBusSystem>();
                    builder.RegisterSceneSystem<SceneServiceSystem>();
                });

                builder.RegisterAssetType<ColorAssetType>();
                builder.RegisterAssetPack(new AssetPackConfiguration()
                {
                    EntryDirectory = DirectoryPath.CurrentDirectory(GuppyGamePack.Directory)
                });
            });
        }
    }
}