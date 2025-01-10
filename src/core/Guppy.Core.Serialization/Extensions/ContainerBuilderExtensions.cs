using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.Serialization.Common.Factories;
using Guppy.Core.Serialization.Common.Services;
using Guppy.Core.Serialization.Factories;
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
            builder.RegisterType<DefaultEnumerableFactory>().As<IDefaultInstanceFactory>().SingleInstance();

            builder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>().InstancePerDependency();
            builder.RegisterGeneric(typeof(PolymorphicJsonSerializerService<>)).As(typeof(IPolymorphicJsonSerializerService<>)).InstancePerDependency();

            builder.RegisterPolymorphicJsonType<bool, object>(nameof(Boolean));
            builder.RegisterPolymorphicJsonType<int, object>(nameof(Int32));
            builder.RegisterPolymorphicJsonType<string, object>(nameof(String));
            builder.RegisterPolymorphicJsonType<LogEventLevel, object>(nameof(LogEventLevel));

            builder.Configure<JsonSerializerOptions>((p, options) =>
            {
                options.PropertyNameCaseInsensitive = true;
                options.WriteIndented = true;
                options.Converters.Add(new JsonStringEnumConverter());

                foreach (JsonConverter converter in p.Resolve<IEnumerable<JsonConverter>>())
                {
                    options.Converters.Add(converter);
                }
            });

            return builder.AddTag(nameof(RegisterCoreSerializationServices));
        }
    }
}