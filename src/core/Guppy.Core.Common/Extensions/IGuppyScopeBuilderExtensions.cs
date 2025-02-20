using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Implementations;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterScopedSystem<TSystem>(this IGuppyScopeBuilder builder)
            where TSystem : IScopedSystem
        {
            builder.RegisterType<TSystem>().As<IScopedSystem>().InstancePerLifetimeScope();

            return builder;
        }

        public static IGuppyScopeBuilder Configure<TConfig>(
            this IGuppyScopeBuilder builder,
            Action<IGuppyScope, TConfig> configurator)
        {
            builder.RegisterInstance(new Configurator<TConfig>(configurator)).As<Configurator>();

            return builder;
        }

        public static IGuppyScopeBuilder Configure<TConfig>(
            this IGuppyScopeBuilder builder,
            Action<TConfig> configurator)
        {
            builder.RegisterInstance(new Configurator<TConfig>((_, instance) => configurator(instance))).As<Configurator>();

            return builder;
        }
    }
}
