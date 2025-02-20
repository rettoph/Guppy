using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.Serialization.Common.Factories;
using Guppy.Core.Serialization.Common.Services;
using Guppy.Core.Serialization.Factories;
using Guppy.Core.Serialization.Services;

namespace Guppy.Core.Serialization.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCoreSerializationServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreSerializationServices), builder =>
            {
                builder.RegisterType<DefaultInstanceService>().As<IDefaultInstanceService>().InstancePerDependency();
                builder.RegisterType<DefaultEnumerableFactory>().As<IDefaultInstanceFactory>().SingleInstance();

                builder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>().InstancePerDependency();
                builder.RegisterGeneric(typeof(PolymorphicJsonSerializerService<>)).As(typeof(IPolymorphicJsonSerializerService<>)).InstancePerDependency();

                builder.RegisterPolymorphicJsonType<bool, object>(nameof(Boolean));
                builder.RegisterPolymorphicJsonType<int, object>(nameof(Int32));
                builder.RegisterPolymorphicJsonType<string, object>(nameof(String));

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
            });
        }
    }
}