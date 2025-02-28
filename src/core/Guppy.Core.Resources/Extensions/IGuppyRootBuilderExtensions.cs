﻿using Autofac;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Common.Serialization.Json;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Core.Resources.Common.Services;
using Guppy.Core.Resources.ResourceTypes;
using Guppy.Core.Resources.Serialization.Json.Converters;
using Guppy.Core.Resources.Services;
using Guppy.Core.Resources.Systems;
using Guppy.Core.Serialization.Common.Extensions;

namespace Guppy.Core.Resources.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCoreResourcesServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreResourcesServices), builder =>
            {
                builder.RegisterGlobalSystem<ResourceServicesInitializationSystem>();

                builder.RegisterType<SettingService>().AsSelf().As<ISettingService>().SingleInstance();
                builder.RegisterType<ResourcePackService>().AsSelf().As<IResourcePackService>().SingleInstance();
                builder.RegisterType<ResourceService>().AsSelf().As<IResourceService>().SingleInstance();
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