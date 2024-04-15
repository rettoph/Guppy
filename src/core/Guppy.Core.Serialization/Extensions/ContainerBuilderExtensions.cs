using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Serialization.Common.Services;
using Guppy.Core.Serialization.Services;

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

            return builder.AddTag(nameof(RegisterCoreSerializationServices));
        }
    }
}
