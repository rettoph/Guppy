﻿using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Resources.Serialization.Json;
using Guppy.Core.Serialization.Common.Services;
using Guppy.Core.Serialization.Services;
using Serilog.Events;

namespace Guppy.Core.Serialization.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreSerializationServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreSerializationServices)))
            {
                return builder;
            }

            builder.RegisterType<DefaultInstanceService>().As<IDefaultInstanceService>().InstancePerDependency();
            builder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>().InstancePerDependency();
            builder.RegisterGeneric(typeof(PolymorphicJsonSerializerService<>)).As(typeof(IPolymorphicJsonSerializerService<>)).InstancePerDependency();

            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<bool, object>(nameof(Boolean))).SingleInstance();
            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<int, object>(nameof(Int32))).SingleInstance();
            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<string, object>(nameof(String))).SingleInstance();
            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<LogEventLevel, object>(nameof(LogEventLevel))).SingleInstance();

            return builder.AddTag(nameof(RegisterCoreSerializationServices));
        }
    }
}
