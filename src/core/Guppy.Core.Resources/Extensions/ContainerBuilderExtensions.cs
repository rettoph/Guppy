﻿using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Files.Common.Serialization.Json;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Services;
using Guppy.Core.Resources.Serialization.Json.Converters;
using Guppy.Core.Resources.Services;
using System.Text.Json.Serialization;

namespace Guppy.Core.Resources.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreResourcesServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreResourcesServices)))
            {
                return builder;
            }

            builder.RegisterType<SettingService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ResourcePackService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ResourceService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ResourceTypeService>().As<IResourceTypeService>().SingleInstance();

            builder.RegisterType<ResourcePacksConfigurationConverter>().As<JsonConverter>().SingleInstance();
            builder.RegisterType<IFileJsonConverter<ResourcePackConfiguration>>().As<JsonConverter>().SingleInstance();
            builder.RegisterType<SettingConverter>().As<JsonConverter>().SingleInstance();

            return builder.AddTag(nameof(RegisterCoreResourcesServices));
        }
    }
}
