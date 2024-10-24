using Autofac;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Constants;
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
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterResource<T>(this ContainerBuilder builder, ResourceKey<T> key, T value, string localization = Localization.en_US)
            where T : notnull
        {
            builder.RegisterInstance(new RuntimeResource<T>(key, value, localization)).As<IRuntimeResource>();

            return builder;
        }

        /// <summary>
        /// Register resource values at runtime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterResource<T>(this ContainerBuilder builder, string key, T value, string localization = Localization.en_US)
            where T : notnull
        {
            builder.RegisterInstance(new RuntimeResource<T>(ResourceKey<T>.Get(key), value, localization)).As<IRuntimeResource>();

            return builder;
        }
    }
}
