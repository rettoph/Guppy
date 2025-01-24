using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Files.Common.Serialization.Json;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions.Autofac;
using Guppy.Core.Resources.Common.Services;
using Guppy.Core.Resources.ResourceTypes;
using Guppy.Core.Resources.Serialization.Json.Converters;
using Guppy.Core.Resources.Services;
using Guppy.Core.Serialization.Common.Extensions;

namespace Guppy.Core.Resources.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreResourcesServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreResourcesServices), builder =>
            {
                builder.RegisterType<SettingService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<ResourcePackService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<ResourceService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<ResourceTypeService>().As<IResourceTypeService>().SingleInstance();

                builder.RegisterJsonConverter<ResourcePacksConfigurationConverter>();
                builder.RegisterJsonConverter<FileJsonConverter<ResourcePackConfiguration>>();
                builder.RegisterJsonConverter<SettingValueConverter>();
                builder.RegisterJsonConverter<ResourceConverter>();
                builder.RegisterJsonConverter<ResourceKeyConverter>();

                builder.RegisterResourceType<StringResourceType>();
            });
        }
    }
}