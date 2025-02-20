using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Implementations;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Common.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterGlobalSystem<TSystem>(this IGuppyRootBuilder builder)
            where TSystem : IGlobalSystem
        {
            builder.RegisterType<TSystem>().As<IGlobalSystem>().SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterScopedSystem<TSystem>(this IGuppyRootBuilder builder)
            where TSystem : IScopedSystem
        {
            builder.RegisterType<TSystem>().As<IScopedSystem>().InstancePerLifetimeScope();

            return builder;
        }

        public static IGuppyRootBuilder Configure<TConfig>(
            this IGuppyRootBuilder builder,
            Action<IGuppyScope, TConfig> configurator)
        {
            builder.RegisterInstance(new Configurator<TConfig>(configurator)).As<Configurator>();

            return builder;
        }

        public static IGuppyRootBuilder Configure<TConfig>(
            this IGuppyRootBuilder builder,
            Action<TConfig> configurator)
        {
            builder.RegisterInstance(new Configurator<TConfig>((_, instance) => configurator(instance))).As<Configurator>();

            return builder;
        }
    }
}