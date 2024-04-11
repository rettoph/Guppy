﻿using Autofac;
using Guppy.Attributes;
using Guppy.Files.Serialization.Json;
using Guppy.Loaders;
using Guppy.Resources.Configuration;
using Guppy.Resources.Serialization.Json;
using Guppy.Resources.Serialization.Json.Converters;
using Guppy.Resources.Services;
using Serilog.Events;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Loaders
{
    [AutoLoad]
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {

            services.RegisterType<SettingService>().As<ISettingService>().SingleInstance();
            services.RegisterType<ResourcePackService>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<ResourceService>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<ResourceTypeService>().As<IResourceTypeService>().SingleInstance();
            services.RegisterGeneric(typeof(PolymorphicJsonSerializerService<>)).As(typeof(IPolymorphicJsonSerializerService<>)).InstancePerDependency();

            services.RegisterType<ResourcePacksConfigurationConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<IFileJsonConverter<ResourcePackConfiguration>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<SettingConverter>().As<JsonConverter>().SingleInstance();

            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<bool, object>(nameof(Boolean))).SingleInstance();
            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<int, object>(nameof(Int32))).SingleInstance();
            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<string, object>(nameof(String))).SingleInstance();
            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<LogEventLevel, object>(nameof(LogEventLevel))).SingleInstance();
        }
    }
}
