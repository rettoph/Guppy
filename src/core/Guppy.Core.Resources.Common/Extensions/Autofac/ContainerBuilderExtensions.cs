using Autofac;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Interfaces;
using Guppy.Core.Resources.Common.Internals;

namespace Guppy.Core.Resources.Common.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterResourcePack(this ContainerBuilder builder, ResourcePackConfiguration configuration)
        {
            builder.RegisterInstance<ResourcePackConfiguration>(configuration);
        }

        /// <summary>
        /// Register resource values at runtime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="resource"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterResourceValue<T>(this ContainerBuilder builder, Resource<T> resource, T value)
            where T : notnull
        {
            builder.RegisterInstance(new RuntimeResourceValue<T>(resource, value)).As<IRuntimeResourceValue>();

            return builder;
        }
    }
}
