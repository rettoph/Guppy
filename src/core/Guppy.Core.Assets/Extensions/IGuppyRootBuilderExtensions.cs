using Autofac;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Common.Serialization.Json;
using Guppy.Core.Assets.Common.Configuration;
using Guppy.Core.Assets.Common.Extensions;
using Guppy.Core.Assets.Common.Services;
using Guppy.Core.Assets.AssetTypes;
using Guppy.Core.Assets.Serialization.Json.Converters;
using Guppy.Core.Assets.Services;
using Guppy.Core.Assets.Systems;
using Guppy.Core.Serialization.Common.Extensions;

namespace Guppy.Core.Assets.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCoreAssetsServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreAssetsServices), builder =>
            {
                builder.RegisterGlobalSystem<AssetServicesInitializationSystem>();

                builder.RegisterType<SettingService>().AsSelf().As<ISettingService>().SingleInstance();
                builder.RegisterType<AssetPackService>().AsSelf().As<IAssetPackService>().SingleInstance();
                builder.RegisterType<AssetService>().AsSelf().As<IAssetService>().SingleInstance();
                builder.RegisterType<AssetTypeService>().As<IAssetTypeService>().SingleInstance();

                builder.RegisterJsonConverter<AssetPacksConfigurationConverter>();
                builder.RegisterJsonConverter<FileJsonConverter<AssetPackConfiguration>>();
                builder.RegisterJsonConverter<SettingValueConverter>();
                builder.RegisterJsonConverter<AssetConverter>();
                builder.RegisterJsonConverter<AssetKeyConverter>();

                builder.RegisterAssetType<StringAssetType>();
            });
        }
    }
}